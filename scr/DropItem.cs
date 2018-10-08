using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class DropItem : MonoBehaviour {

    public string type;
    public string itemId;
    public int value;
    public int partsValue;

    public Text tx;
    public SpriteRenderer sprite;
    LevelGameManager gm;
    string itemName;

    public void MakeGunDrop(string gunId)
    {
        type = "gun";
        itemId = gunId;
        sprite.sprite = Resources.Load("Sprites/Gun/" + GoToGM.Instance.lgm.fieldItemList.GetName(itemId), typeof(Sprite)) as Sprite;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "player") return;
        if (type == "gun")
        {
            gm = GoToGM.Instance.lgm;
            SetGunItemName();
            tx.transform.parent.gameObject.SetActive(true);
        }
    }

    private void SetGunItemName()
    {
        name = gm.fieldItemList.GetName(itemId);
        value = gm.gunList.GetGun(name).value;
        int partsCount = int.Parse(gm.fieldItemList.list[itemId]["parts"]);
        for (int i = 0; i < partsCount; i++)
        {
            //파츠 제작 중
        }
        StringBuilder sb = new StringBuilder();
        sb.Append(LocalizationManager.Instance.GetValue(name + "_N"));
        sb.Append(" (");
        sb.Append(value);
        if (partsValue > 0)
        {
            sb.Append("+");
            sb.Append(partsValue);
        }
        sb.Append(")");
        tx.text = sb.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "player") return;
        if (type == "gun")
        {
            PlayerState ps = gm.playerState;
            if (Input.GetKeyDown("e"))
            {
                if (ps.equippedGun2 == "none" || ps.gunNum ==2)
                {
                    if (ps.gunNum == 1)
                    {
                        ps.equippedGun2 = itemId;
                        Destroy(gameObject);
                    }
                    string tempId = itemId;
                    itemId = ps.equippedGun2;
                    ps.equippedGun2 = tempId;
                    SetGunItemName();
                    sprite.sprite = Resources.Load("Sprites/Gun/" + name, typeof(Sprite)) as Sprite;
                }
                else
                {
                    string tempId = itemId;
                    itemId = ps.equippedGun1;
                    ps.equippedGun1 = tempId;
                    SetGunItemName();
                    sprite.sprite = Resources.Load("Sprites/Gun/" + name, typeof(Sprite)) as Sprite;
                }

            }
            else if (Input.GetKeyDown("f"))
            {
                ps.ChangeScrap(value);
                Destroy(gameObject);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {     
        if (collision.tag != "player") return;
        if (type == "gun")
        {
            tx.transform.parent.gameObject.SetActive(false);
        }
    }
}
