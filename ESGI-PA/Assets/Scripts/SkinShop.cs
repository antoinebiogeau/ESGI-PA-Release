using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SkinShop : MonoBehaviour
{
    public RectTransform contentParent;
    public GameObject skinItemPrefab;

    private List<SkinData> skinDataList;

    void Start()
    {
        if (contentParent == null || skinItemPrefab == null)
        {
            Debug.LogError("ContentParent ou SkinItemPrefab non assigné dans l'inspecteur.");
            return;
        }

        StartCoroutine(GetSkinDataFromAPI());
    }

    IEnumerator GetSkinDataFromAPI()
    {
        Debug.Log("Début de la requête API...");
        using (UnityWebRequest www = UnityWebRequest.Get("http://127.0.0.1:5000/skins"))
        {
            yield return www.SendWebRequest();

            if (!www.isNetworkError && !www.isHttpError)
            {
                string jsonResponse = www.downloadHandler.text;

                // Log the raw server response
                Debug.Log("Réponse JSON reçue : " + jsonResponse);

                Debug.Log("Tentative de déserialization du JSON...");
                // Déclarez skinDataList avant l'appel à FromJson
                SkinDataList skinDataContainer = new SkinDataList();

                try
                {
                    // Maintenant, essayez de désérialiser le JSON dans skinDataContainer
                    skinDataContainer = JsonUtility.FromJson<SkinDataList>(jsonResponse);
                    Debug.Log("Désérialisation du JSON réussie.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Erreur lors de la conversion du JSON : " + ex.Message);
                    Debug.Log("JSON non valide ou format inattendu.");
                }

                if (skinDataContainer == null)
                {
                    Debug.LogError("skinDataContainer est null après avoir tenté de convertir le JSON.");
                }
                else if (skinDataContainer.skins == null)
                {
                    Debug.LogError("skinDataContainer.skins est null après avoir tenté de convertir le JSON.");
                }
                else
                {
                    skinDataList = skinDataContainer.skins;

                    Debug.Log("Données de skins extraites, nombre de skins : " + skinDataList.Count);

                    foreach (SkinData skinData in skinDataList)
                    {
                        GameObject skinItemGO = Instantiate(skinItemPrefab, contentParent).transform.GetChild(0).gameObject;
                        
                        if (skinItemGO == null)
                        {
                            Debug.LogError("skinItemGO est null après l'instantiation.");
                            continue;
                        }

                        SkinItemUI skinItemUI = skinItemGO.GetComponent<SkinItemUI>();
                        if (skinItemUI == null)
                        {
                            Debug.LogError("skinItemUI est null après avoir tenté d'obtenir le composant.");
                            continue;
                        }

                        /*Debug.Log("skinItemUI : " + skinData.name);
                        Debug.Log("skinItemUI : " + skinData.price);
                        Debug.Log("skinItemUI : " + skinData.imagepath);
                        Debug.Log("skinItemUI : " + skinData.description);
                        Debug.Log("skinItemUI : " + skinData.character);
                        Debug.Log("skinItemUI : " + skinData.data_added);
                        Debug.Log("skinItemUI : " + skinData.id);
                        Debug.Log("skinItemUI : " + skinData);*/
                        skinItemUI.Initialize(skinData);
                    }
                }
            }
            else
            {
                Debug.LogError("Erreur lors de la requête API : " + www.error);
            }
        }
    }
}

[System.Serializable]
public class SkinDataList
{
    public List<SkinData> skins;
}

[System.Serializable]
public class SkinData
{
    public int id;
    public string name;
    public int value;
    public string data_added; 
    public string character; 
    public string imagepath;
    public string description;
    public int price;
}

