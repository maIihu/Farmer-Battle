using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class CropController : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private TileBase dat;
    [SerializeField] private TileBase hat;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemap2;

    private GameObject player1Pick;
    private GameObject player2Pick;
    
    private void Start()
    {
        player1Pick = player1.transform.GetChild(1).gameObject;
        player2Pick = player2.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        PlayerController player1C = player1.GetComponentInChildren<PlayerController>();
        PlayerController2 player2C = player2.GetComponentInChildren<PlayerController2>();
        
        if (player1C.isHit)
        {
            Debug.Log("Nhân vật 1 chọn");
            Vector3 pos = player1Pick.transform.position;
            Vector3Int cellPos = tilemap.WorldToCell(pos);
            TileBase currentTile = tilemap.GetTile(cellPos);
            if (currentTile != null) 
            {
                if(currentTile.name is "Tile1" or "Tile2") 
                    tilemap.SetTile(cellPos, dat);
                else if (tilemap.GetTile(cellPos) == dat)
                {
                    GameObject tree = Instantiate(treePrefab, player1Pick.transform.position, 
                        Quaternion.identity, this.tilemap.transform);
                    tree.name = pos.ToString();
                }
            }
            foreach (Transform child in tilemap.transform)
            {
                Crop crop = child.gameObject.GetComponent<Crop>();
                if (crop.isReadyToHarvest && pos.ToString() == child.name)
                {
                    crop.Harvest();
                    player1C.Score++;
                }
            }
        }

        if (player2C.isHit)
        {
            Debug.Log("Nhân vật 2 chọn");
            Vector3 pos = player2Pick.transform.position;
            Vector3Int cellPos = tilemap2.WorldToCell(pos);
            TileBase currentTile = tilemap2.GetTile(cellPos);
            
            if (currentTile != null) 
            {
                if(currentTile.name is "Tile1" or "Tile2") 
                    tilemap2.SetTile(cellPos, dat);
                else if (tilemap2.GetTile(cellPos) == dat)
                {
                    GameObject tree = Instantiate(treePrefab, player2Pick.transform.position, Quaternion.identity,
                        this.tilemap2.transform);
                    tree.name = pos.ToString();
                }
            }
            foreach (Transform child in tilemap2.transform)
            {
                Crop crop = child.gameObject.GetComponent<Crop>();
                if (crop.isReadyToHarvest && pos.ToString() == child.name)
                {
                    crop.Harvest();
                    player2C.Score++;
                }
            }
            
        }
        
    }
    
}
