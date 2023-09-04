using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Camera cam;
    int selected;
    [SerializeField] RectTransform qp;
    [SerializeField] RectTransform ip;
    [SerializeField] RectTransform selector;
    [SerializeField] GameObject landscape;
    [SerializeField] Transform phantom;
    [SerializeField] Image weapon;
    [SerializeField] GameObject slotPref;

    Sprite defaultWeapon;
    Transform content;

    int[] MatsCount = new int[5];
    int[] BlocksCount = new int[5];

    void Awake()
    {
        GameManager.GetInstance().player = gameObject;
    }

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        anim = GetComponentInChildren<Animator>();
        defaultWeapon = weapon.sprite;
        content = ip.GetComponentInChildren<ScrollRect>()
            .transform.GetChild(0).GetChild(0);
    }
    void Update()
    {
        RaycastHit hit2 = new RaycastHit();
        Physics.Raycast(
            cam.transform.position,
            cam.transform.forward,
            out hit2,
            5.0f
        );
        if (hit2.collider && hit2.collider.tag == "Block")
        {
            phantom.gameObject.SetActive(true);
            phantom.position = hit2.transform.position;
        }
        else
        {
            phantom.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0) && anim != null && !Input.GetMouseButton(1))
        {
            anim.SetTrigger("Attack");
            //attack
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(
                cam.transform.position,
                cam.transform.forward,
                out hit,
                5.0f
            );
            HP hp;
            if (hit.collider && (hp = hit
                .collider
                .gameObject
                .GetComponent<HP>()))
            {
                hp.GetDamage(35);
            }
        }
        if (Input.GetMouseButtonDown(0) && BlocksCount[selected] > 0 && Input.GetMouseButton(1))
        {
            //build block
            GameObject.Instantiate(
                landscape.GetComponent<LandscapeGenerator>()
                    .prefabs[selected],
                phantom.position + hit2.normal,
                phantom.rotation,
                landscape.transform
                );
            ChangeBlocks(selected, -1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(
                cam.transform.position,
                cam.transform.forward,
                out hit,
                5.0f
            );
            if (hit.collider != null &&
                hit.collider.gameObject.tag == "Weapon")
            {
                //drop weapon
                DropWeapon();
                //pick up weapon
                hit.collider.transform
                    .SetParent(transform.GetChild(0));

                hit.collider.transform.GetComponent<Animator>().enabled = true;
                anim = hit.collider.transform
                    .GetComponent<Animator>();

                hit.collider.transform
                    .localPosition = Vector3.zero;
                hit.collider.transform
                    .localRotation = Quaternion.identity;
                hit.collider.transform
                    .GetComponent<Rigidbody>()
                    .isKinematic = true;

                weapon.sprite = hit.collider
                    .GetComponent<Weapon>()
                    .icon;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowInventory();
        }

        selected += System.Math.Sign(Input.GetAxis("Mouse ScrollWheel"));
        int k = landscape.GetComponent<LandscapeGenerator>()
                .prefabs.Length;
        // 5, 4->5 but 4->0, 5-->0
        selected = (k + selected) % k;
        // 5, 0->-1 but 0->4, -1-->4

        selector.SetParent(qp.GetChild(6 + selected));
        selector.localPosition = Vector3.zero;
    }
    void DropWeapon()
    {
        if (transform.GetChild(0).childCount > 0)
        {
            transform.GetChild(0).GetChild(0)
                .GetComponent<Rigidbody>()
                .isKinematic = false;
            transform.GetChild(0).GetChild(0)
                .GetComponent<Animator>()
                .enabled = false;
            transform.GetChild(0).GetChild(0)
                .SetParent(null);
            weapon.sprite = defaultWeapon;
        }
    }
    public void ChangeMats(int id, int value)
    {
        MatsCount[id] += value;
        qp.GetChild(id).GetComponentInChildren<Text>().
            text = MatsCount[id].ToString();
    }
    public void ChangeBlocks(int id, int value)
    {
        BlocksCount[id] += value;
        qp.GetChild(6+id).GetComponentInChildren<Text>().
            text = BlocksCount[id].ToString();
    }
    void ShowInventory()
    {
        bool current = ip.gameObject.activeInHierarchy;
        ip.gameObject.SetActive(!current);

        GetComponent<UnityStandardAssets
            .Characters.FirstPerson
            .FirstPersonController>().enabled =
            current;

        if (!current) Cursor.lockState =
                CursorLockMode.None;
        Cursor.visible = !current;

        //тренарные операции
        Time.timeScale = current ? 1 : 0;
        if (!current) InitInventory();
        else ClearInventory();
    }
    void InitInventory()
    {
        for (int i = 0; i < MatsCount.Length; i++)
        {
            Sprite s = qp.GetChild(i).GetChild(0)
                .GetComponent<Image>()
                .sprite;
            for (int j = 0; j < MatsCount[i]; j++)
            {
               GameObject go = GameObject.Instantiate(
                    slotPref,
                    content
                    );
                go.transform.GetChild(0)
                    .GetComponent<Image>()
                    .sprite = s;
                go.transform.GetChild(0).GetComponent<ItemID>().id = qp.GetChild(i).GetChild(0).GetComponent<ItemID>().id;
            }
        }
    }
    void ClearInventory()
    {
        //как перебрать всех потомков трансформа:
        foreach (Transform slot in content)
        {
            Destroy(slot.gameObject);
        }
        ip.GetComponent<UIManager>().CleaCraft();
    }
}
