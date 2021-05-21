// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;

// [RequireComponent(typeof(ARRaycastManager))]
// public class SphereOnTap : MonoBehaviour
// {   
//      public GameObject image1;
//      public GameObject image2;
//      public GameObject image3;
//      public GameObject image4;

//      private GameObject spawnedObject1;
//      private GameObject spawnedObject2;
//      private GameObject spawnedObject3;
//      private GameObject spawnedObject4;
//      private ARRaycastManager _arRaycastManager;
//      private Vector2 touchPosition;
//      static List<ARRaycastHit> hits = new List<ARRaycastHit>();
//     // Start is called before the first frame update

//     private void Awake(){
//         _arRaycastManager = GetComponent<ARRaycastManager>();
//     }
//     bool TryGetTOuchPosition(out Vector2 touchPosition){
//         if(Input.touchCount > 0){
//             touchPosition = Input.GetTouch(0).position;
//             return true;
//         }
//         touchPosition = default;
//         return false;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(!TryGetTOuchPosition(out Vector2 touchPosition))
//             return;
//         if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes)){
//             var hitPose = hits[0].pose;
//             if(spawnedObject1 == null || spawnedObject2 == null || spawnedObject3 == null || spawnedObject4 == null){
//                 var hit1 = new Vector3(hitPose.position.x, hitPose.position.y + 0.038f, hitPose.position.z -0.851f);
//                 var hit2 = new Vector3(hitPose.position.x, hitPose.position.y + 0.038f, hitPose.position.z + 0.823f);
//                 var hit3 = new Vector3(hitPose.position.x, hitPose.position.y + 0.95f, hitPose.position.z - 0.035f);
//                 var hit4 = new Vector3(hitPose.position.x, hitPose.position.y - 0.95f, hitPose.position.z -0.035f);
//                 spawnedObject1 = Instantiate(image1, hit1, hitPose.rotation);
//                 spawnedObject2 = Instantiate(image2, hit2, hitPose.rotation);
//                 spawnedObject3 = Instantiate(image3, hit3, hitPose.rotation);
//                 spawnedObject4 = Instantiate(image4, hit4, hitPose.rotation);
//                 // spawnedObject.transform.position = new Vector3(0f, 0.038f,-0.851f);
//                 spawnedObject1.transform.Rotate(0f, 0f, 0,Space.World);
//                 spawnedObject2.transform.Rotate(0f, 0f, 0,Space.World);
//                 spawnedObject3.transform.Rotate(0f, 0f, 0,Space.World);
//                 spawnedObject4.transform.Rotate(0f, 0f, 0,Space.World);
//             }
//             else{
//                 spawnedObject1.transform.position = hitPose.position;
//             }
//         }
//     }
// }
