using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadNorSane.Characters
{
    class MoveClass
    {
        public static void controlGround(Player player, float T)
        {
            if (player.btn_jump && player.can_jump)
            {
                player.can_jump = false;
                player.my_body.LinearVelocity = new Vector2(player.my_body.LinearVelocity.X, player.jump_speed);
                //player.my_body.ApplyLinearImpulse(new Vector2(0, player.jump_speed));
                return;
            }
            if (!player.btn_move_right && !player.btn_move_left)
            {
                player.my_body.LinearVelocity = new Vector2(0, 0);
                return;
            }
            else
                if(player.btn_move_right && !player.btn_move_left)
                {
                    player.my_body.LinearVelocity = new Vector2(player.move_speed, 0);
                }
                else
                    if (!player.btn_move_right && player.btn_move_left)
                    {
                        player.my_body.LinearVelocity = new Vector2(-player.move_speed, 0);
                    }
        }

        int get_my_current_direction(Player player)
        {
            if(player.my_body.LinearVelocity.X > 0)
            {
                return 1;
            }
            else
                if (player.my_body.LinearVelocity.X < 0)
                {
                    return -1;
                }
            return 0;
        }

        int get_my_wanted_direction(Player player)
        {
            if(player.btn_move_right)
            {
                return 1;
            }
            else
                if(player.btn_move_left)
                {
                    return -1;
                }
            return 0;
        }

        public static void controlAir(Player player, float T)
        {
            if(!player.btn_move_right && !player.btn_move_left)
            {
                player.my_body.ApplyLinearImpulse(new Vector2(0, 0));
                //player.my_body.LinearVelocity = new Vector2(player.my_body.LinearVelocity.X * T, player.my_body.LinearVelocity.Y);
                return;
            }
            else
                if (player.btn_move_right && !player.btn_move_left)
                {
                    if (player.my_body.LinearVelocity.X < player.move_speed)
                    {
                        /*if (get_my_current_direction() != 0 && get_my_wanted_direction() != 0 && get_my_wanted_direction() != get_my_current_direction())
                        {
                            player.my_body.ApplyLinearImpulse(new Vector2(player.move_speed * 0.05f, 0));
                        }
                        else
                        {*/
                            player.my_body.ApplyLinearImpulse(new Vector2(player.move_speed * 0.05f, 0));
                        //}
                    }
                    else
                    {
                        player.my_body.LinearVelocity = new Vector2(player.move_speed, player.my_body.LinearVelocity.Y);
                    }
                    //player.my_body.LinearVelocity = new Vector2(player.move_speed, player.my_body.LinearVelocity.Y);
                }
                else
                    if (!player.btn_move_right && player.btn_move_left)
                    {
                        if (player.my_body.LinearVelocity.X > -player.move_speed)
                        {
                            player.my_body.ApplyLinearImpulse(new Vector2(-player.move_speed * 0.05f, 0));
                        }
                        else
                        {
                            player.my_body.LinearVelocity = new Vector2(-player.move_speed, player.my_body.LinearVelocity.Y);
                        }

                        //player.my_body.LinearVelocity = new Vector2(-player.move_speed, player.my_body.LinearVelocity.Y);
                    }

        }

    }
}
