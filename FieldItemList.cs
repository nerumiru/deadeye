using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FieldItemList : SubLevelManager
{
    //드랍 혹 소지 중인 아이템의 정보드을 관리한다.
    /// <summary>
    /// name, where, sort (gun : leftAmmo, parts, part1)
    /// </summary>
    public Dictionary<string, Dictionary<string,string>> list;
    // Dictionary<string, string> itemInfo;

    //필수
    //name, where, sort
    // where >> field, pocket
    //일부
    //gun : leftAmmo, parts, part1
    // Use this for initialization

    public override void InInitialization()
    {
        list = new Dictionary<string, Dictionary<string, string>>();

        NewGun("g_0", "CursedDartGun", 10);
    }

    public void NewGun(string itemId, string gunName, int bulletNum)
    {
        Dictionary<string, string> temp = new Dictionary<string, string>();
        temp.Add("name", gunName);
        temp.Add("leftAmmo", bulletNum.ToString());
        temp.Add("parts", "0");

        list.Add(itemId,temp);
    }
    public void AddGunPart(string itemId, string partName)
    {
        string count = (int.Parse(list[itemId]["parts"]) + 1).ToString();
        list[itemId]["parts"] = count;
        list[itemId].Add("part" + count, partName);
    }
    
    public string GetName(string item_id)
    {
        Dictionary<string, string> temp;
        if (item_id == "none") return "none";
        if (list.TryGetValue(item_id, out temp))
            return temp["name"];
        else
            return "none";
    }
    public int GetAmmoNum(string item_id)
    {
        Dictionary<string, string> temp;
        int result = 0;
        if (item_id == "none") return result;
        if (!list.TryGetValue(item_id, out temp)) return result;
        if (int.TryParse(temp["leftAmmo"], out result))
            return result;
        else 
            return 0;            
    }
    public void ChangeGun(string dropId, string equipId, int leftAmmo)
    {
        list[dropId]["leftAmmo"] = leftAmmo.ToString();
        list[dropId]["where"] = "field";
        list[equipId]["where"] = "pocket";
    }
    

}
