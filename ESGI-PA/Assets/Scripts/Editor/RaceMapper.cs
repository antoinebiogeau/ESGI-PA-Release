using UnityEditor;
using UnityEngine;

namespace Editor
{
    internal enum RaceObject
    {
        Checkpoint, Box, Respawn
    }

    public class RaceMapper : EditorWindow
    {
        private bool _canPlaceItem;
        private RaceObject _objectToPlace;


        private RaceLoader _loader;

        //Checkpoint
    
        private GameObject _checkpointObject;
        private GameObject _checkpointBatch;
        private int _checkpointCount;
    
        //Respawn

        private RaceConfiguration _raceConfiguration;
    
        //Bonus

        private GameObject _bonusObject;
        private GameObject _bonusBatch;
        private int _bonusCount;
    
        //Positioning and scaling
    
        private string _objectLabel = "None";
        private string _heightOffset = "1";
        private string _scale = "1";
        private bool _validOffset = true;
        private bool _validScale = true;
    
        //Window
    
        [MenuItem("Window/Tools/Race Mapper")]
        public static void ShowWindow()
        {
            var window = GetWindow<RaceMapper>();
            window.titleContent = new GUIContent("Race Mapper");
        }

        private void OnEnable()
        {
            if (Application.isPlaying) return;
            Init();
            SceneView.duringSceneGui += SetObject;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= SetObject;
        }

        private void Init()
        {
            InitCheckpointModule();
            InitRespawnModule();
            InitBonusModule();
        }

        private void InitCheckpointModule()
        {
            _checkpointObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/checkpoint.prefab");
            _checkpointBatch = GameObject.FindGameObjectWithTag("CheckpointBatch");
            if (!_checkpointBatch)
            {
                _checkpointBatch = new GameObject
                {
                    tag = "CheckpointBatch",
                    name = "Checkpoint batch"
                };
                _checkpointCount = 0;
            }
            else
            {
                _checkpointCount = _checkpointBatch.transform.childCount;
            }
        }

        private void InitRespawnModule()
        {
            _raceConfiguration = FindObjectOfType<RaceConfiguration>();
            if (_raceConfiguration) return;
            var raceConfig = new GameObject("Race config");
            var raceConfiguration = raceConfig.AddComponent<RaceConfiguration>();
            _raceConfiguration = raceConfiguration;
        }

        private void InitBonusModule()
        {
            _bonusObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/bonus.prefab");
            _bonusBatch = GameObject.FindGameObjectWithTag("BonusBatch");
            if (!_bonusBatch)
            {
                _bonusBatch = new GameObject
                {
                    tag = "BonusBatch",
                    name = "Bonus batch"
                };
                _bonusCount = 0;
            }
            else
            {
                _bonusCount = _bonusBatch.transform.childCount;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5);
            GUILayout.Label("Mapper", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Checkpoint"))
            {
                _canPlaceItem = true;
                _objectToPlace = RaceObject.Checkpoint;
                _objectLabel = "Checkpoint";
            }
            if (GUILayout.Button("Respawn"))
            {
                _canPlaceItem = true;
                _objectToPlace = RaceObject.Respawn;
                _objectLabel = "Respawn";
            }
            if (GUILayout.Button("Box"))
            {
                _canPlaceItem = true;
                _objectToPlace = RaceObject.Box;
                _objectLabel = "Box";
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
            GUILayout.Label("Advanced options", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ground offset");
            _heightOffset = EditorGUILayout.TextArea(_heightOffset, GUILayout.ExpandHeight(false));
            GUILayout.EndHorizontal();
            if (!_validOffset) EditorGUILayout.LabelField("<color=yellow>[Warning] : The offset is not a number, default offset will be set to 1</color>", new GUIStyle(GUI.skin.label) {richText = true});
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale");
            _scale = EditorGUILayout.TextArea(_scale, GUILayout.ExpandHeight(false));
            GUILayout.EndHorizontal();
            if (!_validScale) EditorGUILayout.LabelField("<color=yellow>[Warning] : The scale is not a number, default scale will be set to 1</color>", new GUIStyle(GUI.skin.label) {richText = true});
            if (_canPlaceItem) GUILayout.Label($"Object placing enabled : {_objectLabel}");
        }

        private void SetObject(SceneView sceneView)
        {
            if (!_canPlaceItem) return;
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1) _canPlaceItem = false;
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 0) return;
            var mousePosition = currentEvent.mousePosition;
            mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
            var ray = sceneView.camera.ScreenPointToRay(mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            var hitPos = hit.point;
            _validOffset = int.TryParse(_heightOffset, out var num);
            hitPos.y += _validOffset ? num : 1;
            _validScale = int.TryParse(_scale, out var scaleValue);
            switch (_objectToPlace)
            {
                case RaceObject.Checkpoint:
                    SetCheckpoint(hitPos, scaleValue);
                    break;
                case RaceObject.Respawn:
                    SetRespawn(hitPos);
                    break;
                case RaceObject.Box:
                    SetBonus(hitPos, scaleValue);
                    break;
            }
            _canPlaceItem = false;
        }
        private void SetCheckpoint(Vector3 hitPos, int scaleValue)
        {
            var checkpoint = Instantiate(_checkpointObject, hitPos, Quaternion.identity);
            checkpoint.name = $"Checkpoint : {_checkpointCount}";
            checkpoint.transform.parent = _checkpointBatch.transform;
            checkpoint.transform.localScale *= _validScale ? scaleValue : 1;
            _checkpointCount++;
        }

        private void SetRespawn(Vector3 hitPos)
        {
            _raceConfiguration.respawnPositions.Add(hitPos);
        }

        private void SetBonus(Vector3 hitPos, int scaleValue)
        {
            var bonus = Instantiate(_bonusObject, hitPos, Quaternion.identity);
            bonus.name = $"Bonus : {_bonusCount}";
            bonus.transform.parent = _bonusBatch.transform;
            bonus.transform.localScale *= _validScale ? scaleValue : 1;
            _bonusCount++;
        }
    }
}