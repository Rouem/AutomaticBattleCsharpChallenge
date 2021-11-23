using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {

        public class ClassAttack
        {
            CharacterClass characterClass;
            
            float abnormalStatusChance = 0.45f;

            float statusDamage = 0;

            AbnormalStatus status = 0;

            public bool ChangeStatus
            {
                get { return abnormalStatusChance >= (float)new Random().NextDouble(); }
            }


            public void SetClass(CharacterClass charclass)
            {
                characterClass = charclass;
                if (characterClass.Equals(CharacterClass.Archer))
                {
                    abnormalStatusChance = 0.25f;
                    status = AbnormalStatus.poisoned;
                    statusDamage = 10;
                }
                else
                if (characterClass.Equals(CharacterClass.Cleric))
                {
                    abnormalStatusChance = 0.2f;
                    var blind = new Random().Next(1, 2) % 2 == 0;
                    status = blind ?
                    AbnormalStatus.blinded : AbnormalStatus.poisoned;
                    statusDamage = blind ? 45 : 12;
                }
                else
                if (characterClass.Equals(CharacterClass.Paladin))
                {
                    abnormalStatusChance = 0.25f;
                    status = AbnormalStatus.paralyzed;
                    statusDamage = 0;
                }
                else
                if (characterClass.Equals(CharacterClass.Warrior))
                {
                    abnormalStatusChance = 0.1f;
                    bool hitkill = new Random().Next(1, 2) % 2 == 0;
                    status = hitkill ? AbnormalStatus.died : AbnormalStatus.died;
                }
            }


            public void AddStatus(Character characterTarget)
            {
                if(!ChangeStatus) return;

                SetClass(characterClass);

                characterTarget.Status = status;
                characterTarget.statusDamage = statusDamage;
                Console.WriteLine($"Player {characterTarget.Name} now is {status}\n");

            }

        }


        //Oct~Nov: Add an effect for each class that can somehow paralyze other characters (random chance) - DONE
        public enum AbnormalStatus : uint
        {
            normal = 0,
            died = 1,
            paralyzed = 2,
            poisoned = 3,
            blinded = 4
        }


        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;

            public GridBox(int x, int y, bool ocupied, int index)
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;
            }

        }


        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

    }
}
