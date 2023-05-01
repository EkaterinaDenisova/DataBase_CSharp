// author Денисова Екатерина
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // для sql

namespace App_DataBase
{
    public partial class FormMain : Form
    {
        // соединение с бд
        private SqlConnection sqlConnection = null;

        // класс, создающий команды, используемые для согласования
        // изменений, произведённых в DataSet, со связанной БД
        private SqlCommandBuilder sqlCommandBuilder = null;

        // класс, представляющий собой набор команд на языке sql,
        // кторые используются для заполнения DataSet и обновления БД
        private SqlDataAdapter sqlDataAdapter = null;

        // класс, представляющий собо кэш данных в памяти
        // используется для обновления БД на сервере
        private DataSet dataSet = null;

        //public Violator violator = new Violator();
        //DBviolators db_violators;

        // логическая переменная
        // если пользователь добавил новую строку, то true
        // иначе false
        private bool newRowAdded = false;

        public FormMain()
        {
            InitializeComponent();
            //db_violators = new DBviolators();
            //dataGridView.DataSource = db_violators;
            //db_violators.add_random_data();
            //violator = new Violator();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Инструкция по работе с базой данных для \r\nрегистрации нарушителей " +
                "правил дорожного движения\r\n1) Чтобы добавить новую строку с данными, введите данные в \r\n" +
                "последней строке и нажмите \"Добавить\" \r\n2) Чтобы удалить строку, нажмите \"Удалить\" \r\n" +
                "3) Чтобы изменить данные в существующей строке, введите новые значения и нажмите \"Изменить\"",
                "Справка");
        }

        // вывести данные в DataGridView из БД на сервере
        private void LoadData()
        {
            try
            {
                // инициализация объекта SqlDataAdapter с командой SELECT и соединением SqlConnection
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Удалить' AS [Command] FROM Violators",sqlConnection);

                // инициализируем sqlCommandBuilder
                // связываем sqlCommandBuilder с sqlDataAdapter
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);

                // генерация команд для добавления, изменения и удаления строк бд
                sqlCommandBuilder.GetInsertCommand();
                sqlCommandBuilder.GetUpdateCommand();
                sqlCommandBuilder.GetDeleteCommand();

                // инициализируем dataSet
                dataSet = new DataSet();

                // Fill - добавление и обновление строк в DataSet
                // "Violators" - название таблицы БД, из которой берутся данные
                sqlDataAdapter.Fill(dataSet, "Violators");

                // DataSource - источник данных, которые отображаются в dataGridView
                dataGridView.DataSource = dataSet.Tables["Violators"];

                // добавим ссылку в последний столбец каждой строки
                // проходим по всем строкам в dataGridView
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    // DataGridViewLinkCell - ячейка, содержащая ссылку
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView[dataGridView.Columns.Count - 1, i] = linkCell;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка при загрузке данных с сервера");
            }
        }

        // обновить данные в DataGridView на основе БД на сервере
        private void ReloadData()
        {
            try
            {
                // очистим данные в dataSet
                dataSet.Tables["Violators"].Clear();

                // Fill - добавление и обновление строк в DataSet
                // "Violators" - название таблицы БД, из которой берутся данные
                sqlDataAdapter.Fill(dataSet, "Violators");

                // DataSource - источник данных, которые отображаются в dataGridView
                dataGridView.DataSource = dataSet.Tables["Violators"];

                // добавим ссылку в последний столбец каждой строки
                // проходим по всем строкам в dataGridView
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    // DataGridViewLinkCell - ячейка, содержащая ссылку
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView[dataGridView.Columns.Count - 1, i] = linkCell;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при загрузке данных с сервера");
            }
        }

        // при загрузке формы
        private void FormMain_Load(object sender, EventArgs e)
        {
            // соединение с бд с помощью строки подключения
            sqlConnection = new SqlConnection(@"Data Source =.\SQLEXPRESS; AttachDbFilename = C:\Users\denis\Desktop\db\App_DataBase\App_DataBase\Database1.mdf; Integrated Security = True; User Instance = True");
        
            // открытие подключения к бд
            sqlConnection.Open();

            LoadData();

            // запретить сортировку последнего столбца (Command)
            dataGridView.Columns[dataGridView.Columns.Count - 1].SortMode = DataGridViewColumnSortMode.NotSortable;

        }

        // обработчик события нажатия на ячейку dataGridView
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // если был нажат заголовок столбца
                if (e.RowIndex == -1)
                {

                }
                // если была нажата ячейка в столбце "Command"
                else if (e.ColumnIndex == dataGridView.Columns.Count - 1)
                {
                    // получаем текст (команду) из последней колонки
                    string comand = dataGridView.Rows[e.RowIndex].Cells[dataGridView.Columns.Count - 1].Value.ToString();
                    
                    if (comand == "Удалить")
                    {
                        // если пользователь согласен на удаление строки из БД
                        if (MessageBox.Show("Вы уверены, что хотите удалить эту строку?", "Удаление строки", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            // индекс строки, которую нужно удалить
                            int rowIndex = e.RowIndex;

                            // удаляем строку из dataGridView
                            dataGridView.Rows.RemoveAt(rowIndex);

                            // удаляем строку из dataSet
                            dataSet.Tables["Violators"].Rows[rowIndex].Delete();

                            // обновляем БД на сервере в соответствии с dataSet
                            sqlDataAdapter.Update(dataSet, "Violators");



                        }
                    }
                    else if (comand == "Добавить")
                    {
                        // индекс добавляемой строки 
                        int rowIndex = dataGridView.Rows.Count - 2;

                        // DataRow - строка данных таблицы
                        // NewRow создаёт новую строку таблицы (тип DataRow) с той же схемой
                        // что и в таблице dataSet.Tables["Violators"]
                        DataRow row = dataSet.Tables["Violators"].NewRow();
                        Violator violator = new Violator();
                        /*dataGridView.Rows[rowIndex].Cells["Name"].Value.ToString(),
                        dataGridView.Rows[rowIndex].Cells["Surname"].Value.ToString(),
                        dataGridView.Rows[rowIndex].Cells["Patronymic"].Value.ToString(),
                        dataGridView.Rows[rowIndex].Cells["Car_number"].Value.ToString(),
                        dataGridView.Rows[rowIndex].Cells["Car_number"].Value.ToString(),

                        );*/
                        violator.SetSurname(dataGridView.Rows[rowIndex].Cells["Surname"].Value.ToString());
                        violator.SetName(dataGridView.Rows[rowIndex].Cells["Name"].Value.ToString());
                        violator.SetPatronymic(dataGridView.Rows[rowIndex].Cells["Patronymic"].Value.ToString());
                        violator.SetCarNumber(dataGridView.Rows[rowIndex].Cells["Car_number"].Value.ToString());
                        violator.SetReceiptNumber(Convert.ToInt32(dataGridView.Rows[rowIndex].Cells["Receipt_number"].Value));
                        violator.SetPayment(Convert.ToDouble(dataGridView.Rows[rowIndex].Cells["Payment"].Value));

                        //row["Name"] = dataGridView.Rows[rowIndex].Cells["Name"].Value;

                        row["Name"] = violator.GetName();
                        row["Surname"] = violator.GetSurname();
                        row["Patronymic"] = violator.GetPatronymic();
                        row["Car_number"] = violator.GetCarNumber();
                        row["Receipt_number"] = violator.GetReceiptNumber();
                        row["Payment"] = violator.GetPayment();

                        // добавляем строку в dataSet
                        dataSet.Tables["Violators"].Rows.Add(row);

                        dataSet.Tables["Violators"].Rows.RemoveAt(dataSet.Tables["Violators"].Rows.Count - 1);
                        dataGridView.Rows.RemoveAt(dataGridView.Rows.Count - 2);


                        dataGridView.Rows[e.RowIndex].Cells[dataGridView.Columns.Count - 1].Value = "Удалить";

                        sqlDataAdapter.Update(dataSet, "Violators");

                        newRowAdded = false;
                    }
                    else if (comand == "Изменить")
                    {
                        int rowIndex = e.RowIndex;

                        // DataRow - строка данных таблицы
                        // NewRow создаёт новую строку таблицы (тип DataRow) с той же схемой
                        // что и в таблице dataSet.Tables["Violators"]
                        //DataRow row = dataSet.Tables["Violators"].NewRow();

                        Violator violator = new Violator();

                        violator.SetSurname(dataGridView.Rows[rowIndex].Cells["Surname"].Value.ToString());
                        violator.SetName(dataGridView.Rows[rowIndex].Cells["Name"].Value.ToString());
                        violator.SetPatronymic(dataGridView.Rows[rowIndex].Cells["Patronymic"].Value.ToString());
                        violator.SetCarNumber(dataGridView.Rows[rowIndex].Cells["Car_number"].Value.ToString());
                        violator.SetReceiptNumber(Convert.ToInt32(dataGridView.Rows[rowIndex].Cells["Receipt_number"].Value));
                        violator.SetPayment(Convert.ToDouble(dataGridView.Rows[rowIndex].Cells["Payment"].Value));

                        /*row["Name"] = violator.GetName();
                        row["Surname"] = violator.GetSurname();
                        row["Patronymic"] = violator.GetPatronymic();
                        row["Car_number"] = violator.GetCarNumber();
                        row["Receipt_number"] = violator.GetReceiptNumber();
                        row["Payment"] = violator.GetPayment();*/

                        //dataSet.Tables
                        dataSet.Tables["Violators"].Rows[rowIndex]["Name"] = violator.GetName();
                        dataSet.Tables["Violators"].Rows[rowIndex]["Surname"] = violator.GetSurname();
                        dataSet.Tables["Violators"].Rows[rowIndex]["Patronymic"] = violator.GetPatronymic();
                        dataSet.Tables["Violators"].Rows[rowIndex]["Car_number"] = violator.GetCarNumber();
                        dataSet.Tables["Violators"].Rows[rowIndex]["Receipt_number"] = violator.GetReceiptNumber();
                        dataSet.Tables["Violators"].Rows[rowIndex]["Payment"] = violator.GetPayment();

                        sqlDataAdapter.Update(dataSet, "Violators");

                        dataGridView.Rows[e.RowIndex].Cells[dataGridView.Columns.Count - 1].Value = "Удалить";
                    }
                    

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при изменении данных");
            }
        }

        // обработчик события добавления новой строки в dataGridView пользователем
        private void dataGridView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdded == false)
                {
                    // newRowAdded равно true, когда пользователь добавляет строку
                    newRowAdded = true;

                    // индекс добавленной строки
                    int lastRow = dataGridView.Rows.Count - 2;

                    DataGridViewRow row = dataGridView.Rows[lastRow];

                    // DataGridViewLinkCell - ячейка, содержащая ссылку
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView[dataGridView.Columns.Count - 1, lastRow] = linkCell;

                    // текст в последней колонке заменяется с "Удалить" на "Добавить"
                    row.Cells[dataGridView.Columns.Count - 1].Value = "Добавить";

                }
            }
            catch
            {

            }
        }

        // если данные в таблице были изменены
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(newRowAdded == false)
                {
                    // получаем индекс строки выделенной ячейки
                    int rowIndex = dataGridView.SelectedCells[0].RowIndex;

                    // сохраняем редактируемую строку в editRow
                    DataGridViewRow editRow = dataGridView.Rows[rowIndex];

                    // DataGridViewLinkCell - ячейка, содержащая ссылку
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView[dataGridView.Columns.Count - 1, rowIndex] = linkCell;

                    // текст в последней колонке заменяется с "Удалить" на "Добавить"
                    editRow.Cells[dataGridView.Columns.Count - 1].Value = "Изменить";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при изменении данных");
            }
        }

        // обработчик события при отображении элемента управления для редактирования ячейки
        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);

            // 5 и 6 - индексы столбцов, в которых нужно проверять
            // допустимость вводимых значений
            if ((dataGridView.CurrentCell.ColumnIndex == 5) ||
                (dataGridView.CurrentCell.ColumnIndex == 6) )
            {
                // Control отображает область для пользователя,
                // где он может ввести или изменить значение
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    // KeyPressEventHandler - метод, обрабатывающий событие KeyPress
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            // IsControl - относится ли символ к управляющим
            // IsDigit - является ли символ цифрой
            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
    }
}
