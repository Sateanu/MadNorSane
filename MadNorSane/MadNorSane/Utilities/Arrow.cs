using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MadNorSane.Characters;
using FarseerPhysics.Dynamics;
using Krypton;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;

namespace MadNorSane.Utilities
{
  public class Arrow:Physics_object
    {
      Player owner;
      public Arrow(World _new_world,ContentManager _new_content,Player owner,Vector2 direction)
      {
          this.owner = owner;
          _my_content = _new_content;
          my_world = _new_world;
          my_body = BodyFactory.CreateRectangle(my_world, 0.5f, 0.5f, 1, owner.my_body.Position);
          my_body.UserData = "arrow";
          width = 0.5f;
          height = 0.5f;
          my_body.Rotation = (float)Math.Atan2(direction.X, -direction.Y);
          my_body.FixedRotation=true;
          my_body.OnCollision += my_body_OnCollision;
          my_body.BodyType = BodyType.Dynamic;
          my_body.IgnoreGravity = true;
          my_body.Mass = 1;
          my_body.ApplyLinearImpulse(direction);
          set_texture("arrow");
      }
      bool my_body_OnCollision(Fixture fixA, Fixture fixB, FarseerPhysics.Dynamics.Contacts.Contact contact)
      {
          Vector2 touched_sides = contact.Manifold.LocalNormal;
          if (contact.IsTouching)
          {

              if (fixA.Body.UserData=="arrow")
              {
                  if (fixB.Body.UserData == owner)
                      return false;
                  else
                      if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                      {
                          Console.WriteLine("Sageata a lovit player");
                          my_body.UserData = "arrow_dropped";
                          fixA.Body.LinearVelocity = Vector2.Zero;
                          fixA.Body.IgnoreGravity = false;
                          fixA.Body.Rotation = 0f;
                          return false;
                      }
                      else
                          if (fixB.Body.UserData == "ground" || fixB.Body.UserData == "wall")
                          {
                              my_body.UserData = "arrow_dropped";
                              fixA.Body.LinearVelocity = Vector2.Zero;

                              fixA.Body.IgnoreGravity = false;
                              fixA.Body.Rotation = 0f;
                              return true;
                          }
                          else if (fixB.Body.UserData == "arrow" || fixB.Body.UserData == "arrow_dropped")
                              return false;
              }
              else
                  if (fixA.Body.UserData=="arrow_dropped")
                  {
                      if (fixB.Body.UserData == owner)
                      {
                          fixA.Body.Dispose();
                          fixA.Dispose();
                          Active = false;
                      }
                      else
                          if (fixB.Body.UserData.GetType().IsSubclassOf(typeof(Player)))
                          {
                              return false;
                          }
                          else
                              if (fixB.Body.UserData == "ground" || fixB.Body.UserData == "wall")
                                  return true;
                              else if (fixB.Body.UserData == "arrow" || fixB.Body.UserData == "arrow_dropped")
                                  return false;

                  }
                  
          }
          return true;
      }
    }
}
