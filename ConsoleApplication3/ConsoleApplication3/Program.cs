using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FarForFolderAndFiles
{
    class CustomFolderInfo
    {
        CustomFolderInfo prev;
        int index;
        FileSystemInfo[] objs;

        public CustomFolderInfo(CustomFolderInfo prev, int index, FileSystemInfo[] list)
        {
            this.prev = prev;
            this.index = index;
            this.objs = list;
        }

        public void PrintFolderInfo()
        {
            Console.Clear();

            for (int i = 0; i < objs.Length; ++i)
            {
                if (objs[i].GetType() == typeof(FileInfo))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (objs[i].GetType() == typeof(DirectoryInfo))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                Console.WriteLine(objs[i]);
                Console.BackgroundColor = ConsoleColor.DarkBlue;


            }
        }

        public void DecreaseIndex()
        {
            if (index >= 0) index--;
            if (index < 0) index = objs.Length - 1;

        }

        public void IncreaseIndex()
        {
            if (index < objs.Length) index++;
            if (index == objs.Length) index = 0;
        }

        public CustomFolderInfo GetNextItem()
        {
            FileSystemInfo active = objs[index];
            List<FileSystemInfo> list = new List<FileSystemInfo>();
            DirectoryInfo d = (DirectoryInfo)active;
            list.AddRange(d.GetDirectories());
            list.AddRange(d.GetFiles());
            CustomFolderInfo x = new CustomFolderInfo(this, 0, list.ToArray());
            return x;
        }

        public CustomFolderInfo GetPrevItem()
        {
            return prev;
        }
        class Program
        {
            static void ShowFolderInfo(CustomFolderInfo item)
            {

                item.PrintFolderInfo();

                ConsoleKeyInfo pressedKey = Console.ReadKey();

                if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    item.DecreaseIndex();
                    ShowFolderInfo(item);
                }
                else if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    item.IncreaseIndex();
                    ShowFolderInfo(item);
                }
                else if (pressedKey.Key == ConsoleKey.Enter)
                {
                    if (item.objs[item.index].GetType() == typeof(DirectoryInfo))

                    {
                        CustomFolderInfo newItem = item.GetNextItem();
                        ShowFolderInfo(newItem);
                    }
                    else
                    {

                        if (item.objs[item.index].Extension == ".txt")
                        {
                            Console.Clear();
                            FileInfo d = (FileInfo)item.objs[item.index];

                            string line;
                            List<FileSystemInfo> list = new List<FileSystemInfo>();
                            string s = d.FullName;
                            StreamReader file = new StreamReader(s);
                            while ((line = file.ReadLine()) != null)
                            {
                                Console.Clear();
                                Console.WriteLine(line);
                            }
                            ConsoleKeyInfo newpressedKey = Console.ReadKey();
                            if (newpressedKey.Key == ConsoleKey.Escape)
                            {
                                ShowFolderInfo(item);
                            }
                        }
                    }
                }
                else if (pressedKey.Key == ConsoleKey.Escape)
                {

                    CustomFolderInfo newItem = item.GetPrevItem();
                    ShowFolderInfo(newItem);


                }

            }
            static void Main(string[] args)
            {
               
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                List<FileSystemInfo> list = new List<FileSystemInfo>();
                var d = new DirectoryInfo(@"C:\Users\Lenovo\Desktop\work");
                list.AddRange(d.GetDirectories());
                list.AddRange(d.GetFiles());

                CustomFolderInfo test = new CustomFolderInfo(null, 0, list.ToArray());

                ShowFolderInfo(test);


            }
        }
    }
}
