﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeusExThings.Projectiles
{
    class GEPGunWPRocketProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 3600;
            projectile.aiStyle = 1;
            projectile.light = 1f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            projectile.Kill();
            return false;
        }

        public override void AI()
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];
                if (!target.friendly)
                {
                    float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
                    float shootToY = target.position.Y - projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                    if (distance < 800f && !target.friendly && target.active)
                    {
                        distance = 3f / distance;

                        shootToX *= distance * 5;
                        shootToY *= distance * 5;

                        projectile.velocity.X = shootToX;
                        projectile.velocity.Y = shootToY;
                    }
                }

                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, Color.White, 1f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            // Play explosion sound
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Projectiles/GasGrenadeExplode"));

            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            for (int g = 0; g < 2; g++)
            {
                int goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
                goreIndex = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                Main.gore[goreIndex].scale = 1.5f;
                Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
                Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
            }
            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        /*public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            The debuff inflicted is the modded debuff ~~Ethereal Flames~~ On Fire!. x is the duration in frames: Terraria runs at 60 FPS, 
            so that's t seconds (x/60=t). To change the modded debuff, change EtherealFlames to whatever the buff is called; to add a vanilla debuff, 
            change mod.BuffType("EtherealFlames") to a number based on the terraria buff IDs. 
            Some useful ones are 20 for poison, 24 for On Fire!, 39 for Cursed Flames, 69 for Ichor, and 70 for Venom.
            target.AddBuff(BuffID.OnFire, 600);
            /*target.AddBuff(mod.BuffType("CursedInferno"), 600, false);
            target.AddBuff(mod.BuffType("ShadowFlame"), 600, false);
            target.AddBuff(mod.BuffType("Ichor"), 600, false);
            target.AddBuff(mod.BuffType("Venom"), 600, false);
            target.AddBuff(mod.BuffType("BrokenArmor"), 600, false);
            target.AddBuff(mod.BuffType("Confusion"), 600, false);
        }*/
    }
}
