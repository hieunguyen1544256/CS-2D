using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUltis
{
    public class Option
    {
        public bool isDontDestroy = false;
        public bool isTransform = false;
        public Vector3 offset = Vector3.zero;
        public float timeDestroy = 2;
        public float scaleRate = 1;
        public Vector3? startScale;
        public Vector3? position;
    }

    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;
        public EffectGame[] effects;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public ParticleSystem CreatedEffect(string name, Transform t, Option option = null)
        {
            option ??= new();

            EffectGame e = Array.Find(effects, x => x.name == name);

            if (e == null)
            {
                Debug.Log("Effect Not Found: " + name);
                return null;
            }
            else
            {
                ParticleSystem e1 = Instantiate(e.effect);
                if (option.isTransform)
                {
                    e1.transform.SetParent(t);
                }

                if (t != null)
                {
                    e1.transform.position = option.position ?? (t.position + option.offset);
                }

                e1.transform.localScale = option?.startScale ?? e1.transform.localScale * option.scaleRate;


                e1.Play();

                if (!option.isDontDestroy)
                {
                    Destroy(e1.gameObject, option.timeDestroy);
                }

                return e1;
            }
        }

        public void CreatedEffect(string name, Vector3 position, Option option = null)
        {
            option ??= new();

            EffectGame e = Array.Find(effects, x => x.name == name);

            if (e == null)
            {
                Debug.Log("Effect Not Found: " + name);
            }
            else
            {
                ParticleSystem e1 = Instantiate(e.effect);
                e1.transform.position = position + option.offset;

                e1.Play();

                if (!option.isDontDestroy)
                {
                    Destroy(e1.gameObject, option.timeDestroy);
                }

            }
        }

        public ParticleSystem CreatedEffect(ParticleSystem e, Transform t, Option option = null)
        {
            option ??= new();

            if (e == null)
            {
                Debug.Log("Effect is null");
                return null;
            }
            else
            {
                ParticleSystem e1 = Instantiate(e);
                if (option.isTransform)
                {
                    e1.transform.SetParent(t);
                }
                e1.transform.position = option.position ?? (t.position + option.offset);
                e1.transform.localScale = option?.startScale ?? e1.transform.localScale * option.scaleRate;


                e1.Play();

                if (!option.isDontDestroy)
                {
                    Destroy(e1.gameObject, option.timeDestroy);
                }

                return e1;
            }
        }
    }

    [Serializable]
    public class EffectGame
    {
        public string name;
        public ParticleSystem effect;
    }
}
