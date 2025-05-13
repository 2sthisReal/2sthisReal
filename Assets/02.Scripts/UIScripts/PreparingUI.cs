using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SWScene
{
    public class PreparingUI : BaseUI
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button mainMenuButton;

        [SerializeField] private Image weaponImage;
        [SerializeField] private Image armorImage;
        [SerializeField] private Image accessory1Image;
        [SerializeField] private Image accessory2Image;
        [SerializeField] private Image pet1Image;
        [SerializeField] private Image pet2Image;

        [SerializeField] private Button[] inventoryButton = new Button[6];
        List<EquipmentData> equipmentList;
        List<Image> equipmentImageList = new();
        Dictionary<EquipmentSlot, Image> EquipmentSlotMap = new();

        protected override void Awake()
        {
            base.Awake();
            EquipmentSlotMap[EquipmentSlot.Weapon] = weaponImage;
            EquipmentSlotMap[EquipmentSlot.Armor] = armorImage;
            EquipmentSlotMap[EquipmentSlot.Accessory1] = accessory1Image;
            EquipmentSlotMap[EquipmentSlot.Accessory2] = accessory2Image;    
            EquipmentSlotMap[EquipmentSlot.Pet1] = pet1Image;    
            EquipmentSlotMap[EquipmentSlot.Pet2] = pet2Image;

            equipmentImageList.Add(weaponImage);
            equipmentImageList.Add(armorImage);
            equipmentImageList.Add(accessory1Image);
            equipmentImageList.Add(accessory2Image);
            equipmentImageList.Add(pet1Image);
            equipmentImageList.Add(pet2Image);
        }


        protected override GameState GetUIState()
        {
            return GameState.Preparing;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);
            /*if (scene.name == "PreparingScene")
            {
                equipmentList = UIManager.instance.equipmentDatabase.AllEquipments;
                for(int i = 0; i < 6; i++)
                {
                    if (equipmentList.Count > i)
                    {
                        Image image = InventoryButton[i].image;
                        Debug.Log(equipmentList[i].iconPath);
                        image.sprite = Resources.Load<Sprite>(equipmentList[i].iconPath);
                    }
                }
            }*/
        }

        protected override void Start()
        {
            base.Start();
            equipmentList = UIManager.instance.equipmentDatabase.AllEquipments;
            for (int i = 0; i < 6; i++)
            {
                if (equipmentList.Count > i)
                {
                    Image image = inventoryButton[i].image;
                    image.sprite = Resources.Load<Sprite>(equipmentList[i].iconPath);
                    int index = i;
                    inventoryButton[index].onClick.AddListener(() =>
                    {
                        if (equipmentList.Count > index)
                        {
                            GameManager.Instance.Equipment.AutoAssign(equipmentList[index]);
                        }
                        foreach(var equipment in GameManager.Instance.Equipment.GetAll())
                        {
                            EquipmentSlotMap[equipment.Key].sprite = Resources.Load<Sprite>(equipment.Value.iconPath);
                        }
                    });
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected override void Update()
        {
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);
            startButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.InGame);
                });
            mainMenuButton.onClick.AddListener(
                () =>
                {
                    GameManager.Instance.ChangeState(GameState.MainMenu);
                });
        }
    }
}