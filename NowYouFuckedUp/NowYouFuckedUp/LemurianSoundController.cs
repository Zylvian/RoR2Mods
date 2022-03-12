using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections;
using static R2API.SoundAPI;

namespace NowYouFuckedUp
{
    public class LemurianSoundController : MonoBehaviour
    {

        uint eventId;
        readonly uint soundId = 1351821836;

        public void Awake()
        {


            using (var bankStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NowYouFuckedUp.FuckedUpBank.bnk"))
            {
                var bytes = new byte[bankStream.Length];
                bankStream.Read(bytes, 0, bytes.Length);
                SoundBanks.Add(bytes);
            }
        }

        public void OnEnable()
        {
            eventId = AkSoundEngine.PostEvent(soundId, this.gameObject);
        }

        public void OnDisable()
        {
            AkSoundEngine.StopPlayingID(eventId);
        }


    }
}
