using System.Linq;
using DefaultNamespace.Game;
using DefaultNamespace.Game.Plants;
using DTO;
using Game.Dirty;
using Menu;
using Menu.Shop_Component;
using ServiceScript;
using SO;
using UnityEngine;

namespace LoadScenes
{
    public class GameSceneLoad : RegisterServicesScene
    {
        public const string FieldFenceId = "LevelField";
        public Product<PlayerMark> DefaultPlayer;

        public C_GameScene Controller;
        public Level DefaultLevel;
        public Field FieldForFence;
        public FenceManager FenceManager;
        public ProgressLevel ProgressLevel;
        public UIMediatorGame MediatorUI;
        public ZoneManager ZoneManager;
        public GreenManager GreenManager;
        public UfoTransiter Ufo;

        public Transform PointForVirtualCamera;

        private Progress Progress => Services<DB>.S.Get().Progress;
        private ProductContainer Products => Services<ProductContainer>.S.Get();

        public override void Register()
        {
            Services<C_GameScene>.S.Set(Controller);
            if (Services<Level>.S.Get() == null) Services<Level>.S.Set(DefaultLevel);
            Services<FenceManager>.S.Set(FenceManager);
            Services<ProgressLevel>.S.Set(ProgressLevel);
            Services<UIMediatorGame>.S.Set(MediatorUI);
            Services<PlayerMark>.S.Set(SpawnPlayer());
            Services<ZoneManager>.S.Set(ZoneManager);
            Services<GreenManager>.S.Set(GreenManager);
            Services<UfoTransiter>.S.Set(Ufo);
            

            var levelField = Instantiate(Services<Level>.S.Get().Field);
            FieldForFence.transform.position = levelField.transform.position;
            FieldForFence.InitField(levelField.Size);
            FieldForFence.Init();
            ServicesID<Field>.S.Set(levelField);
            ServicesID<Field>.S.Set(FieldForFence, FieldFenceId);

            Services<CacheField>.S.Set(levelField.GetComponent<CacheField>());
        }

        private PlayerMark SpawnPlayer()
        {
            if(string.IsNullOrWhiteSpace(Progress.GetSelectedGuidProduct<PlayerMark>())) Progress.SetSelectedGuidProduct<PlayerMark>(DefaultPlayer);

            var playerPrefab = Products.Players.FirstOrDefault(x => x.GUID == Progress.GetSelectedGuidProduct<PlayerMark>());
            if (playerPrefab == null)
            {
                Progress.SetSelectedGuidProduct<PlayerMark>(DefaultPlayer);
                playerPrefab = DefaultPlayer as PlayerProduct;
            }
            var result = Instantiate(playerPrefab.Content);
            PointForVirtualCamera.SetParent(result.transform);
            PointForVirtualCamera.localPosition=Vector3.zero;

            return result;
        }

        public override void Unregister()
        {
            ServicesID<Field>.S.Set(null);
            ServicesID<Field>.S.Set(null, FieldFenceId);
        }
    }
}