using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace CheesyDrive
{
    public class Program
    {
        private static CTRE.TalonSrx leftDrive;
        private static CTRE.TalonSrx rightDrive;
        private const float TURNING_CONSTANT = 0.6f;

        public static void CheesyDrive(float throttle, float steer)
        {
            float coeff = 1.0f;
            float steeringComponent = steer * TURNING_CONSTANT;
            float throttleComponent = throttle * coeff;

            leftDrive.Set(throttleComponent + steeringComponent);
            rightDrive.Set(throttleComponent - steeringComponent);
        }

        public static void Main()
        {
            leftDrive = new CTRE.TalonSrx(1);
            rightDrive = new CTRE.TalonSrx(2);
            leftDrive.SetInverted(true);

            leftDrive.SetControlMode(CTRE.TalonSrx.ControlMode.kPercentVbus);
            rightDrive.SetControlMode(CTRE.TalonSrx.ControlMode.kPercentVbus);
            leftDrive.SetFeedbackDevice(CTRE.TalonSrx.FeedbackDevice.QuadEncoder);
            rightDrive.SetFeedbackDevice(CTRE.TalonSrx.FeedbackDevice.QuadEncoder);
            leftDrive.ConfigEncoderCodesPerRev(560);
            rightDrive.ConfigEncoderCodesPerRev(560);
            leftDrive.SetSensorDirection(true);
            leftDrive.SetEncPosition(0);
            rightDrive.SetEncPosition(0);

            CTRE.Gamepad gamePad = new CTRE.Gamepad(new CTRE.UsbHostDevice());
            /* loop forever */
            while (true)
            {
                if( gamePad.GetConnectionStatus() == CTRE.UsbDeviceConnection.Connected)
                {
                    CheesyDrive(gamePad.GetAxis(1), gamePad.GetAxis(2));
                    CTRE.Watchdog.Feed();
                    Debug.Print("" + leftDrive.GetPosition() + ":" + rightDrive.GetPosition());
                }
                else
                {
                    Debug.Print("No Driver Pad");
                }
                /* wait a bit */
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
