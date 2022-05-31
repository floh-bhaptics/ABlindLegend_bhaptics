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
        public enum directions { front, back, left, right };
        public static directions attackDirection = directions.front;

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
                tactsuitVr.LOG("Attack " + attackDirection.ToString() + " " + other.strength.ToString() + " " + other.type.ToString());
                tactsuitVr.PlaybackHaptics("Slice_" + attackDirection.ToString());
            }
        }

        [HarmonyPatch(typeof(Combat), "Initialize", new Type[] {  })]
        public class bhaptics_InitCombat
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StartHeartBeat();
            }
        }

        [HarmonyPatch(typeof(Combat), "Destroy", new Type[] { })]
        public class bhaptics_EndCombat
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                tactsuitVr.StopHeartBeat();
                tactsuitVr.PlaybackHaptics("StoreSword_R");
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
                {
                    tactsuitVr.LOG("Protect");
                    tactsuitVr.PlaybackHaptics("BlockHands_L");
                    tactsuitVr.PlaybackHaptics("BlockArms_L");
                    tactsuitVr.PlaybackHaptics("BlockVest_L");
                    return;
                }
                    
                if (___input.justDid(InputActionName.DRAW) != null)
                {
                    tactsuitVr.LOG("Draw");
                    tactsuitVr.PlaybackHaptics("DrawSword_R");
                }
                    
                if (___input.justDid(InputActionName.PUSHAWAY) != null)
                {
                    tactsuitVr.LOG("PushAway");
                    tactsuitVr.PlaybackHaptics("PushAway");
                }
                if (___input.justDid(InputActionName.ATTACK_BEHIND) != null)
                    attackDirection = directions.back;
                if (___input.justDid(InputActionName.ATTACK_FRONT) != null)
                    attackDirection = directions.front;
                if (___input.justDid(InputActionName.ATTACK_LEFT) != null)
                    attackDirection = directions.left;
                if (___input.justDid(InputActionName.ATTACK_RIGHT) != null)
                    attackDirection = directions.right;
            }
        }

        [HarmonyPatch(typeof(CombatSystem), "HitCurrentEnemy", new Type[] { typeof(float) })]
        public class bhaptics_PlayerHitsEnemy
        {
            [HarmonyPostfix]
            public static void Postfix(CombatSystem __instance)
            {
                tactsuitVr.LOG("HitCurrentEnemy");
                tactsuitVr.Recoil("Sword", true);
            }
        }

    }

}
