using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project2
{
    interface Controls
    {
        void YawLeft();
        void YawRight();
        void RollLeft();
        void RollRight();
        void BankLeft();
        void BankRight();
        void PitchUp();
        void PitchDown();
        void ThroatleUp();
        void ThroatleDown();
        void FlipForward();
        void FlipBackwards();
        void ShootGun();
        void ChangeGun();
        void ShootMissle();
        void ChangeMissle();
        void Reload();
        void Boost();
    }
}
