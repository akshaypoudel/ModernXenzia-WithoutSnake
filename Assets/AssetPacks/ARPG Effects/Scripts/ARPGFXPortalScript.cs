using UnityEngine;
using System.Collections;

namespace ARPGFX
{


    public class ARPGFXPortalScript : MonoBehaviour
    {

        public GameObject portalOpenPrefab;
        public GameObject portalIdlePrefab;
        public GameObject portalClosePrefab;
        private GameObject portalOpen;
        private GameObject portalIdle;
        private GameObject portalClose;
        public Vector3 sizeOfPortals;
        private bool canRun = true;

        public float portalLifetime = 4.0f;


        void Start()
        {
            portalOpen = Instantiate(portalOpenPrefab, transform.position, transform.rotation);
            portalIdle = Instantiate(portalIdlePrefab, transform.position, transform.rotation);
            portalIdle.SetActive(false);
            portalClose = Instantiate(portalClosePrefab, transform.position, transform.rotation);
            portalClose.SetActive(false);

            SetSize(portalOpen,portalIdle,portalClose);
            StartCoroutine("PortalLoop");
        }

        private void Update()
        {   
            if(!this.gameObject.activeSelf)
            {
                Destroy(this.gameObject);
                Destroy(portalOpen);  
                Destroy(portalIdle);
                Destroy(portalClose);
            }
        }

        IEnumerator PortalLoop()
        {
            while (canRun)
            {
                portalOpen.SetActive(true);

                yield return new WaitForSeconds(0.8f);

                portalIdle.SetActive(true);
                portalOpen.SetActive(false);

                yield return new WaitForSeconds(portalLifetime);

                portalIdle.SetActive(false);

                portalClose.SetActive(true);

                yield return new WaitForSeconds(1f);

                portalClose.SetActive(false);
            }
        }

        void SetSize(GameObject portalOpen,GameObject portalIdle,GameObject portalClose)
        {
            portalOpen.transform.localScale = sizeOfPortals;
            portalIdle.transform.localScale=sizeOfPortals;
            portalClose.transform.localScale=sizeOfPortals; 
        }
    }

}