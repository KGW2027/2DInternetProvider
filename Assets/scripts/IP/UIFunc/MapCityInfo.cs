using System;
using IP.Objective.Builds;
using UnityEngine;

namespace IP.UIFunc
{
    public class MapCityInfo : MonoBehaviour
    {
        public SpriteRenderer starlink;
        public SpriteRenderer cache;
        public SpriteRenderer idc;
        public SpriteRenderer office;
        public SpriteRenderer asCenter;

        private void Start()
        {
            starlink.gameObject.SetActive(false);
            cache.gameObject.SetActive(false);
            idc.gameObject.SetActive(false);
            office.gameObject.SetActive(false);
            asCenter.gameObject.SetActive(false);
        }

        public void ActiveIcon(BuildBase build)
        {
            if(build is Starlink && !starlink.gameObject.activeSelf) starlink.gameObject.SetActive(true);
            else if(build is CacheServer && !cache.gameObject.activeSelf) cache.gameObject.SetActive(true);
            else if(build is HeadOffice or Office && !office.gameObject.activeSelf) office.gameObject.SetActive(true);
            else if(build is ServiceCenter && !asCenter.gameObject.activeSelf) asCenter.gameObject.SetActive(true);
        }

        public void ActiveIcon(BuildBase build, Texture2D idcTier)
        {
            idc.gameObject.SetActive(true);
            Sprite idcSprite = Sprite.Create(idcTier, new Rect(0, 0, idcTier.width, idcTier.height), new(.5f, .5f));
            idc.size = new(4, 4);
            idc.sprite = idcSprite;
        }
    }
}