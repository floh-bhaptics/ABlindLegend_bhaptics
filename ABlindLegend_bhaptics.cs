using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;

namespace ABlindLegend_bhaptics
{
    public class ABlindLegend_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(Hero), "ReceiveHit", new Type[] { typeof(EnemyStats), typeof(bool), typeof(bool)})]
        public class bhaptics_PlayerReceivesHit
        {
            [HarmonyPostfix]
            public static void Postfix(Hero __instance, EnemyStats other)
            {
                tactsuitVr.LOG("Attack strength" + other.strength.ToString());
            }
        }

        [HarmonyPatch(typeof(HeroStats), "RemoveLife", new Type[] {  })]
        public class bhaptics_PlayerLives
        {
            [HarmonyPostfix]
            public static void Postfix(HeroStats __instance)
            {
                int totalLives = __instance.lives + __instance.boughtLives;
                if (totalLives == 1) tactsuitVr.StartHeartBeat();
                else tactsuitVr.StopHeartBeat();
            }
        }

        [HarmonyPatch(typeof(HurtArea), "Enter", new Type[] { typeof(UnityEngine.Collider) })]
        public class bhaptics_PlayerHurtArea
        {
            [HarmonyPostfix]
            public static void Postfix(HurtArea __instance)
            {
                tactsuitVr.LOG("HurtArea: " + __instance.areaName);
            }
        }
        
        [HarmonyPatch(typeof(Combat), "Update", new Type[] { })]
        public class bhaptics_PlayerCombat
        {
            [HarmonyPostfix]
            public static void Postfix(Combat __instance, InputController ___input)
            {
                if (___input.justDid(InputActionName.PROTECT) != null)
                    tactsuitVr.LOG("Protect");
                if (___input.justDid(InputActionName.DRAW) != null)
                    tactsuitVr.LOG("Draw");
                if (___input.justDid(InputActionName.PUSHAWAY) != null)
                    tactsuitVr.LOG("PushAway");
                if (___input.justDid(InputActionName.ATTACK_BEHIND) != null)
                    tactsuitVr.LOG("AttackBehind");
                if (___input.justDid(InputActionName.ATTACK_FRONT) != null)
                    tactsuitVr.LOG("AttackFront");
                if (___input.justDid(InputActionName.ATTACK_LEFT) != null)
                    tactsuitVr.LOG("AttackLeft");
                if (___input.justDid(InputActionName.ATTACK_RIGHT) != null)
                    tactsuitVr.LOG("AttackRight");
            }
        }

        [HarmonyPatch(typeof(CombatSystem), "HitCurrentEnemy", new Type[] { typeof(float) })]
        public class bhaptics_PlayerHitsEnemy
        {
            [HarmonyPostfix]
            public static void Postfix(CombatSystem __instance)
            {
                tactsuitVr.LOG("HitCurrentEnemy");
            }
        }

    }

}
