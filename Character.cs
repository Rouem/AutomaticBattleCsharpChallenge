using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        //Make sure all the variables in CHARACTER are engaged in a code feature - DONE
        public string Name { get; set; }

        public float Health;

        public float BaseDamage;
        
        public float DamageMultiplier { get; set; }

        public GridBox currentBox;

        public int PlayerIndex;

        public AbnormalStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        private Grid battlefield;

        public Character Target { get; set; }

        private CharacterClass characterClass;

        private ClassAttack classAttack = new ClassAttack();

        private AbnormalStatus status = 0;

        public float statusDamage = 0;


        public Character(CharacterClass characterClass)
        {
            this.characterClass = characterClass;
            classAttack.SetClass(characterClass);
        }


        public void TakeDamage(float amount)
        {
            if ((Health -= amount) <= 0)
            {
                Die();
            }
        }


        public void Die()
        {
            //TODO >> maybe kill him?
            //Console.WriteLine($"{PlayerIndex} - Player {Name} is defeated!!!");

            status = AbnormalStatus.died;
            Health = 0;
        }


        public void Move()
        {
            currentBox.ocupied = false;
            battlefield.grids[currentBox.Index] = currentBox;
            currentBox = GetDirection();
            currentBox.ocupied = true;
            battlefield.grids[currentBox.Index] = currentBox;
        }


        public void StartTurn(Grid battlefield)
        {
            this.battlefield = battlefield;
            CheckStatus();
        }

        
        public void Attack(Character target)
        {

            if (Target.Health <= 0) return;

            if (Health <= 0 || status.Equals(AbnormalStatus.died)) return;

            var rand = new Random();
            if (status.Equals(AbnormalStatus.blinded))
                if (statusDamage >= rand.Next(0, 100))
                {
                    Console.WriteLine($"Player {Name} is attacking the player {Target.Name} but miss it...");
                    return;
                }

            DamageMultiplier = (float)rand.NextDouble();
            var totalDamage = BaseDamage + (BaseDamage * DamageMultiplier);
            Console.WriteLine($"Player {Name} is attacking the player {Target.Name} and did {(int)totalDamage} damage.");
            target.TakeDamage((int)totalDamage);
            classAttack.AddStatus(target);
        }


        private GridBox GetDirection()
        {

            if (this.currentBox.xIndex > Target.currentBox.xIndex)
            {
                if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                {
                    Console.WriteLine($"Player {Name} - ({characterClass}) walked LEFT\n");
                    return (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                }
            }
            else
            if (currentBox.xIndex < Target.currentBox.xIndex)
            {
                Console.WriteLine($"Player {Name} - ({characterClass}) walked RIGHT\n");
                return (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
            }

            int compensation = battlefield.xLenght - battlefield.yLength;
            if (this.currentBox.yIndex > Target.currentBox.yIndex)
            {
                Console.WriteLine($"Player {Name} - ({characterClass}) walked UP\n");
                return (battlefield.grids.Find(x => x.Index - compensation == (currentBox.Index) - battlefield.xLenght));
            }
            else
            //if (this.currentBox.yIndex < Target.currentBox.yIndex)
            {
                Console.WriteLine($"Player {Name} - ({characterClass}) walked DOWN\n");
                return (battlefield.grids.Find(x => x.Index + compensation == (currentBox.Index) + battlefield.xLenght));
            }
        }


        private void CheckStatus()
        {
            if (Health <= 0)
                status = AbnormalStatus.died;
                
            if (status.Equals(AbnormalStatus.normal))
            {
                DoAction();
                return;
            }
            else
            if (status.Equals(AbnormalStatus.died))
            {
                Console.WriteLine($"{PlayerIndex} - Player {Name} take a fatal hit!!!");
                Die();
                return;
            }
            else
            if (status.Equals(AbnormalStatus.paralyzed))
            {
                Console.WriteLine($"{PlayerIndex} - Player {Name} is [{status}] and lost the turn.");
                status = AbnormalStatus.normal;
                return;
            }
            if (status.Equals(AbnormalStatus.poisoned))
            {
                Console.WriteLine($"{PlayerIndex} - Player {Name} is [{status}] and lost {statusDamage} of Health.");
                Health -= statusDamage;
                status = AbnormalStatus.normal;
                DoAction();
                return;
            }
            if (status.Equals(AbnormalStatus.blinded))
            {
                Console.WriteLine($"{PlayerIndex} - Player {Name} is [{status}] on this turn.");
                DoAction();
                status = AbnormalStatus.normal;
                return;
            }
        }


        private void DoAction()
        {

            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);
                return;
            }
            else
            {
                Move();

                //The battlefield should only be reprinted/redrawn if a player makes a move - DONE
                battlefield.drawBattlefield(
                    new List<Character>() { this, Target }
                );

                // Each team should have one move per turn (except when the move places 
                // the character in attack range of an opposing team character) - DONE
                if (CheckCloseTargets(battlefield))
                    Attack(Target);

                return;
            }
        }


        // Check in x and y directions if there is any character close enough to be a target.
        private bool CheckCloseTargets(Grid battlefield)
        {
            int compensation = (int)MathF.Abs(battlefield.xLenght - battlefield.yLength);
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index + compensation == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index - compensation == currentBox.Index - battlefield.xLenght).ocupied);

            //Each character should look for a possible target and attack it when this is viable 
            //and if not, move closer into attack range - DONE

            if (left || right || up || down)
            {
                return true;
            }
            return false;
        }


    }
}
