using BepInEx;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using BepInEx.AssemblyPublicizer;
using System.Reflection;
using System;

namespace ValheimModArmorSkills
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class ValheimModArmorSkills : BaseUnityPlugin
    {
        public const string PluginGUID = "com.bhixsonsimeral.armorskills";
        public const string PluginName = "Armor Skills";
        public const string PluginVersion = "0.0.1";
        
        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        // Test assets
        private Texture2D TestTex;
        private Sprite TestSprite;

        // Custom skill
        private Skills.SkillType TestSkill = 0;

        private void Awake()
        {
            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ValheimModArmorSkills has landed");

            AddSkills();
            Player.m_localPlayer.m_onDamaged += RaiseSkillLevel;
        }

        // Various forms of asset loading
        private void LoadAssets()
        {
            // path to the folder where the mod dll is located
            string modPath = Path.GetDirectoryName(Info.Location);

            // Load texture from the filesystem
            TestTex = AssetUtils.LoadTexture(Path.Combine(modPath, "Assets/test_tex.jpg"));
            TestSprite = Sprite.Create(TestTex, new Rect(0f, 0f, TestTex.width, TestTex.height), Vector2.zero);
        }

        // Add a new test skill
        void AddSkills()
        {
            // Test adding a skill with a texture
            Sprite testSkillSprite = Sprite.Create(TestTex, new Rect(0f, 0f, TestTex.width, TestTex.height), Vector2.zero);
            List<SkillConfig> skillConfigs = new List<SkillConfig>();
            skillConfigs.Add(new SkillConfig
                {
                    Identifier = "com.jotunn.ArmorSkills.testskill",
                    Name = "TestingSkill",
                    Description = "A nice testing skill!",
                    Icon = testSkillSprite,
                    IncreaseStep = 1f
                }
            );
            
            foreach (SkillConfig skillConfig in skillConfigs)
            {
                TestSkill = SkillManager.Instance.AddSkill(skillConfig);
            }
        }

        void RaiseSkillLevel(HitData hitData)
        {
            if (hitData.m_hitType == HitData.HitType.EnemyHit)
            {
                Player.m_localPlayer.RaiseSkill(TestSkill);
            }
        }

        void RaiseSkillLevel(float damage, Character attacker)
        {
            if(attacker != null)
            {
                Player.m_localPlayer.RaiseSkill(TestSkill);
            }
        }
    }
}

