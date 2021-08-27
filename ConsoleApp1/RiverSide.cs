using System;
using System.Collections.Generic;
using System.Linq;

namespace WolfGoatCabbage
{
    /// <summary>
    /// Класс берега, инкапсулирующий список пассажиров 
    /// и методы для добавления, удаления, проверки находящихся на берегу пассажиров.
    /// </summary>
    class RiverSide
    {
        // список пассажиров на берегу
        private List<IPassenger> passengers;
        public int PassengersCount 
        { 
            get { return passengers.Count; } 
        }

        /// <summary>
        /// Конструктор, инициализирующий список пассажиров на берегу значением по умолчанию.
        /// </summary>
        public RiverSide()
        {
            passengers = new List<IPassenger>();
        }

        /// <summary>
        /// Конструктор, принимающий список пассажиров, которые находятся на берегу.
        /// </summary>
        /// <param name="boatPassengers">Список пассажиров.</param>
        public RiverSide(params IPassenger[] boatPassengers)
        {
            passengers = new List<IPassenger>(boatPassengers);
        }

        /// <summary>
        /// Индексатор, возвращающий пассажира по индексу согласно количеству пассажиров на берегу.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPassenger this[int index]
        {
            get
            {
                if (index >= 0 && index < passengers.Count)
                {
                    return passengers[index];
                }
                throw new ArgumentOutOfRangeException("Индекс за пределами диапазона значений");
            }
        }

        /// <summary>
        /// Содержится ли на берегу указанный пассажир.
        /// </summary>
        /// <param name="psgr">Пассажир.</param>
        /// <returns>
        /// Значение true - tсли указанный пассажир содержится на берегу.
        /// В противном случае - значение false.
        /// </returns>
        public bool Contains(IPassenger psgr)
        {
            return passengers.Contains(psgr);
        }

        /// <summary>
        /// Строковое представление пассажиров, находящихся на берегу.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string allPassengers = string.Empty;
            allPassengers = passengers.Select((item, index) => $"{index + 1} {item} \n")
                .Aggregate("", (s, i) => s += i);
            return allPassengers;
        }

        /// <summary>
        /// Посадка пассажира на берег.
        /// </summary>
        /// <param name="psgr">Пассажир.</param>
        public void AddPassenger(IPassenger psgr)
        {
            passengers.Add(psgr);
        }

        /// <summary>
        /// Посадка пассажира с берега на лодку.
        /// </summary>
        /// <param name="psgr">Пассажир.</param>
        /// <param name="message">
        /// Сообщение о неверном пользовательском выборе пассажира для перепавы.
        /// Другие пассажиры на берегу не могут оставаться вместе.
        /// </param>
        /// <returns>
        /// Значение true - если пассажиры могут остаться вместе на берегу.
        /// В проивном случае значение false.
        /// </returns>
        public bool RemovePassenger(IPassenger psgr, out string message)
        {
            message = string.Empty;
            if (passengers.Contains(psgr))
            {
                if (CheckPassengers(psgr, out message))
                {
                    passengers.Remove(psgr);
                    return true;
                }
                return false;
            }
            message = "На берегу нет такого пассажира";
            return false;
        }

        /// <summary>
        /// Проверить, можно ли оставить пассажиров вместе на берегу, если мужик никого с собой не берет.
        /// </summary>
        /// <param name="message">Сообщение о невозможности оставить пассажиров вместе на берегу.</param>
        /// <returns>
        /// Значение true, если пассажиров можно оставить вместе на берегу.
        /// В противном случае - значение false.
        /// </returns>
        public bool CheckPassengers(out string message)
        {
            message = string.Empty;
            if (passengers.Exists(x => x.GetType() == typeof(Goat)) &&
                    passengers.Exists(x => x.GetType() == typeof(Wolf)))
            {
                message = "Волк съест козу. Повторите выбор.";
                return false;
            }
            else if (passengers.Exists(x => x.GetType() == typeof(Goat)) &&
                passengers.Exists(x => x.GetType() == typeof(Cabbage)))
            {
                message = "Коза съест капусту. Повторите выбор.";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверить, можно ли при посадке в лодку пассажира для переправы оставить других вместе на берегу.
        /// </summary>
        /// <param name="psgr">Выбранный пассажир для переправы.</param>
        /// <param name="message">Сообщение о неверном выборе пассажира для переправы.</param>
        /// <returns>
        /// Значение true - при верном выборе пассажира для переправы.
        /// В противном случае - значение false.
        /// </returns>
        private bool CheckPassengers(IPassenger psgr, out string message)
        {
            message = string.Empty;
            List<IPassenger> remainingPassengers =
                passengers.Where(x => x.GetType() != psgr.GetType()).ToList();
            if (remainingPassengers.Count == 2)
            {
                if (remainingPassengers.Exists(x => x.GetType() == typeof(Goat)) &&
                    remainingPassengers.Exists(x => x.GetType() == typeof(Wolf)))
                {
                    message = "Волк съест козу. Повторите выбор.";
                    return false;
                }
                else if (remainingPassengers.Exists(x => x.GetType() == typeof(Goat)) &&
                    remainingPassengers.Exists(x => x.GetType() == typeof(Cabbage)))
                {
                    message = "Коза съест капусту. Повторите выбор.";
                    return false;
                }
                return true;
            }
            return true;
        }
    }
}
