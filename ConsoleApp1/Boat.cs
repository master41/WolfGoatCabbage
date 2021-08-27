using System;

namespace WolfGoatCabbage
{
    /// <summary>
    /// Перечисление, инкапсулирующие состояние 
    /// текущего берега - левый или правый.
    /// </summary>
    enum RiverSideState
    {
        Left,
        Right
    }

    /// <summary>
    /// Класс лодки, инкапсулирующий два берега и процесс переправы пассажиров.
    /// </summary>
    class Boat
    {
        private Man man;

        private RiverSide left;
        private RiverSide right;

        // начальный берег
        private RiverSideState startSide;
        // текущий берег 
        private RiverSideState currentSide;

        // пассажиры переправлены при значении true
        public bool IsTransferEnd
        {
            get 
            {
                switch (startSide)
                {
                    case RiverSideState.Left:
                        if (right.PassengersCount == 3)
                        { return true; }
                        break;
                    case RiverSideState.Right:
                        if (left.PassengersCount == 3)
                        { return true; }
                        break;
                }
                return false;
            }
        }
        

        public Boat(Man m, RiverSide l, RiverSide r, RiverSideState start)
        {
            man = m;
            left = l;
            right = r;
            startSide = start;
            currentSide = startSide;
            Display(startSide);
        }

        /// <summary>
        /// Процесс переправы всех пассажиров пользователем 
        /// на противоположный берег.
        /// </summary>
        public void Transfer()
        {
            while (!IsTransferEnd)
            {
                TransferToOtherSide();
            }
            DisplaySuccess();
        }

        /// <summary>
        /// Вывести сообщение о переправе всех пассажиров.
        /// </summary>
        private void DisplaySuccess()
        {
            ConsoleColor previousForeGround = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nЗадача решена.");
            Console.ForegroundColor = previousForeGround;
        }
        
        /// <summary>
        /// Вывод сообщение об ошибке.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        private void DisplayWarning(string message)
        {
            ConsoleColor previousForeGround = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = previousForeGround;
        }

        /// <summary>
        /// Обработка введенного пользователем индекса выбраного пассажира 
        /// для переправы. Индекс относительно числа пассажиров на данном берегу.
        /// </summary>
        /// <param name="riverSideFrom">Берег от которого переправляются</param>
        /// <returns>
        /// Индекс выбраного пользователем пассажира для переправы. 
        /// Индекс относительно числа пассажиров на данном берегу.
        /// </returns>
        private int InputPassengerForTransfer(RiverSide riverSideFrom)
        {
            bool passengerInput = false;
            int passengerNumber = 0;

            while (!passengerInput)
            {
                string riverSideText = currentSide == RiverSideState.Left ?
                    "правый" : "левый";
                Console.WriteLine($"\nКого перевозим на {riverSideText} берег?" +
                    $"\n{riverSideFrom}" +
                    $"{riverSideFrom.PassengersCount + 1} Переправиться самому\n");

                string input = string.Empty;
                input = Console.ReadLine();
                passengerInput = int.TryParse(input, out passengerNumber) &&
                passengerNumber >= 1 && passengerNumber <= riverSideFrom.PassengersCount + 1;
            }
            return passengerNumber;
        }

        /// <summary>
        /// Переправить пассажира на противоположный берег.
        /// </summary>
        private void TransferToOtherSide()
        {
            while (true)
            {
                // переключение значений начального и конечного берега
                // на текущем этапе переправы

                RiverSide riverSideFrom;
                RiverSide riverSideTo;
                switch (currentSide)
                {
                    case RiverSideState.Left:
                        riverSideFrom = left;
                        riverSideTo = right;
                        break;
                    case RiverSideState.Right:
                        riverSideFrom = right;
                        riverSideTo = left;
                        break;
                    default:
                        return;
                }

                // индекс выбранного пользователем
                // пассажира согласно их количеству на берегу
                int passengerNumber = InputPassengerForTransfer(riverSideFrom);                

                // сообщение об ошибке
                string message = string.Empty;

                // переправить мужика с пассажиром на противоположный берег

                if (passengerNumber != riverSideFrom.PassengersCount + 1)
                {
                    IPassenger psgr = riverSideFrom[passengerNumber - 1];
                    // проверить, можно ли оставить пассажиров на берегу
                    if (riverSideFrom.RemovePassenger(psgr, out message))
                    {
                        // отобразить переправу пассажирова лодки через реку
                        DisplayTransfer(currentSide, psgr);
                        // посадка переправленого пассажира 
                        // на противоположный берег
                        riverSideTo.AddPassenger(psgr);
                        // переключить значение берега, у которого находится лодка
                        SwitchSide();
                        // отобразить пассажиров на берегах после посадки нового 
                        // пассажира
                        Display(currentSide);
                        return;
                    }
                    // вывод сообщения о неверном выборе пользователя
                    // пассажира для переправы 
                    DisplayWarning(message);
                }

                // переправить мужика без пассажиров на противоположный берег 

                else
                {
                    // проверить, можно ли оставить пассажиров на берегу
                    if (riverSideFrom.CheckPassengers(out message))
                    {
                        DisplayTransfer(currentSide);                        
                        SwitchSide();
                        Display(currentSide);
                        return;
                    }
                    // вывод сообщения о неверном выборе пользователя
                    // пассажира для переправы 
                    DisplayWarning(message);
                }
            }
        }

        /// <summary>
        /// Переключить значение берега, у которого находится лодка.
        /// </summary>
        private void SwitchSide()
        {
            switch (currentSide)
            {
                case RiverSideState.Left:
                    currentSide = RiverSideState.Right;
                    break;
                case RiverSideState.Right:
                    currentSide = RiverSideState.Left;
                    break;
            }
        }

        /// <summary>
        /// Отобразить пасажиров на берегу и в лодке во время переправы.
        /// </summary>
        /// <param name="riverSideFromState">Берег, от которого переправляют пассажира.</param>
        /// <param name="psgr">Переправляемый пассажир.</param>
        private void DisplayTransfer(RiverSideState riverSideFromState, IPassenger psgr = null)
        {
            string direction = DisplayBoatPassengersAtRiver(riverSideFromState, psgr);
            DrawRiverSides(direction);
        }

        /// <summary>
        /// Отобразить пассажиров на берегу и лодку с мужиком после посадки.
        /// </summary>
        /// <param name="riverSideState">Берег, у которого находится лодка.</param>
        private void Display(RiverSideState riverSideState)
        {
            string direction = DisplayBoatAtSide(riverSideState);            
            DrawRiverSides(direction);
        }

        /// <summary>
        /// Отобразить пассажиров на берегу и в лодке.
        /// </summary>
        /// <param name="direction">Строка, отображающая лодку с пассажирами.</param>
        private void DrawRiverSides(string direction)
        {
            // отображение левого берега

            ConsoleColor previousBackGroundColor = Console.BackgroundColor;
            ConsoleColor previousForeGroundColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("{0,-30}", left.ToString().Replace('\n', ' '));
            Console.BackgroundColor = previousBackGroundColor;

            // отображение реки и лодки

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(direction);
            Console.BackgroundColor = previousBackGroundColor;

            // отображение правого берега

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write("{0,-30}\n", right.ToString().Replace('\n', ' '));
            Console.BackgroundColor = previousBackGroundColor;
            Console.ForegroundColor = previousForeGroundColor;
        }
        
        /// <summary>
        /// Отобразить лодку с мужиком у берега во время посадки пассажиров.
        /// </summary>
        /// <param name="riverSideState">Берег, у которого находится лодка.</param>
        /// <returns>Строка, отображающая лодку у берега, на который выполняется посадка.</returns>
        private string DisplayBoatAtSide(RiverSideState riverSideState)
        {
            string boat = string.Empty;
            switch (riverSideState)
            {
                case RiverSideState.Left:
                    boat = string.Format("{0, -15}", man);
                    break;
                case RiverSideState.Right:
                    boat = string.Format("{0, 15}", man);
                    break;
            }
            return boat;
        }

        /// <summary>
        /// Отобразить лодку с мужиком и пассажирами во время переправы.
        /// </summary>
        /// <param name="riverSideFromState">Берег, от которого выполняется переправа.</param>
        /// <param name="psgr">Список пассажиров лодки.</param>
        /// <returns>Строка, отображающая пассажиров лодки во время переправы.</returns>
        private string DisplayBoatPassengersAtRiver(RiverSideState riverSideFromState, 
            IPassenger psgr = null)
        {
            string direction = string.Empty;
            string passenger;
            if (psgr == null)
            {
                passenger = string.Empty;
            }
            else
            {
                passenger = psgr.ToString();
            }
            
            switch (riverSideFromState)
            {
                case RiverSideState.Left:
                    direction = string.Format("{0, -15}", man + passenger + "->");
                    break;
                case RiverSideState.Right:
                    direction = string.Format("{0, 15}", "<-" + man + passenger);
                    break;
            }
            return direction;
        }
    }
}
