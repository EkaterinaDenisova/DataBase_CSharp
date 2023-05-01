// author Денисова Екатерина
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_DataBase
{
    public class Violator
    {
        private string surname;      // фамилия нарушителя
        private string name;         // имя нарушителя
        private string patronymic;   // отчество нарушителя
        private string car_number;   // номер автомобиля
        private int receipt_number;  // номер квитанции
        private double payment;      // сумма оплаты

        public Violator()
        {
            SetSurname("");
            SetName("");
            SetPatronymic("");
            SetCarNumber("");
            SetReceiptNumber(0);
            SetPayment(0);
        }

        public Violator(string surname, string name, string patronymic, string car_number, int receipt_number, double payment)
        {
            
            SetSurname(surname);
            SetName(name);
            SetPatronymic(patronymic);
            SetCarNumber(car_number);
            SetReceiptNumber(receipt_number);
            SetPayment(payment);
        }

        public void SetName(string _name)
        {
            name = _name;
        }

        public string GetName()
        {
            return name;
        }

        public void SetSurname(string _surname)
        {
            surname = _surname;
        }

        public string GetSurname()
        {
            return surname;
        }

        public void SetPatronymic(string _patronymic)
        {
            patronymic = _patronymic;
        }

        public string GetPatronymic()
        {
            return patronymic;
        }

        public void SetCarNumber(string _car_number)
        {
            car_number = _car_number;
        }

        public string GetCarNumber()
        {
            return car_number;
        }

        public void SetReceiptNumber(int _receipt_number)
        {
            receipt_number = _receipt_number;
        }

        public int GetReceiptNumber()
        {
            return receipt_number;
        }

        public void SetPayment(double _payment)
        {
            payment = _payment;
        }

        public double GetPayment()
        {
            return payment;
        }

    }
}
