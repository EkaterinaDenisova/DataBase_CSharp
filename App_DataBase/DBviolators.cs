// author Денисова Екатерина
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;       // для ObservableCollection
// ObservableCollection -- коллекция, которую можно использовать совместно с DataGrid
// Эта коллекция можеть оповещать о своём изменении DataGrid
// DataGrid, в свою очередь, автоматически поддерживает сортировки и т.п.

namespace App_DataBase
{
    public class DBviolators
    {
        //public DataSet dataset = null;
        public ObservableCollection<Violator> coll_violators;

        public DBviolators()
        {
            coll_violators = new ObservableCollection<Violator>();
        }



        /// Добавление случайной записи в таблицу
        public void add_random_data()
        {
            Random rnd = new Random();
            // todo: static member
            //List<String> names = new List<string> { "Гвидо ван Россум ", "Бьёрн Страуструп", "Деннис Ритчи", "Дональд Эрвин Кнут" };

            coll_violators.Add(new Violator("Петров", "Денис", "Сергеевич", "а888аа", 754, 5000));
        }

    }

    
}
