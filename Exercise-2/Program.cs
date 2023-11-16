using System;
using System.Collections.Generic;
using System.IO;

namespace laba2
{
    internal class NegativeDiscriminantException : Exception
    {
        public override string Message
        {
            get
            {
                return "Дискриминант меньше 0";
            }
        }
    }

    internal class ZeroDiscriminantException : Exception
    {
        public override string Message
        {
            get
            {
                return "Дискриминант равен 0";
            }
        }
    }

    internal class IncorrectDataException : Exception
    {
        public override string Message
        {
            get
            {
                return "Неверный формат данных";
            }
        }
    }

    public struct EqData {

        EqData(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public double A;
        public double B;
        public double C;
    }

    public struct EqRoots
    {
        EqRoots(double x1, double x2)
        {
            X1 = x1;
            X2 = x2;
        }
        public double? X1;
        public double? X2;
    }

    class Program
    {
        static public double calculate_discriminant(EqData equation_data)
        {
            
            return equation_data.B * equation_data.B - 4 * equation_data.A * equation_data.C;
        }

        static public EqData parse_data(StreamReader data)
        {
            EqData equation_data = new EqData();
            if (!double.TryParse(data.ReadLine(), out equation_data.A) || !double.TryParse(data.ReadLine(), out equation_data.B) || !double.TryParse(data.ReadLine(), out equation_data.C))
                throw new IncorrectDataException();
            return equation_data;

        }

        static public EqRoots calculate_equation(EqData equation_data)
        {
            double discriminant = calculate_discriminant(equation_data);
            EqRoots roots = new EqRoots();

            if (discriminant > 0)
            {
                roots.X1 = (-equation_data.B - Math.Sqrt(discriminant)) / (2 * equation_data.A);
                roots.X2 = (-equation_data.B + Math.Sqrt(discriminant)) / (2 * equation_data.A);

            }
            else if (discriminant == 0)
            {
                roots.X1 = (-equation_data.B) / (2 * equation_data.A);
            }

            return roots;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Программа для решение квадратного уравнения: ");
            bool success = false;
            EqRoots? roots = null;
            try {
                Console.Write("Введите путь к файлу: ");
                StreamReader data = new StreamReader(Console.ReadLine());
                EqData equation_data = parse_data(data);
                double discriminant = calculate_discriminant(equation_data);
                roots = calculate_equation(equation_data);
                if (discriminant < 0)
                    throw new NegativeDiscriminantException();
                else if (discriminant == 0)
                    throw new ZeroDiscriminantException();
                success = true;
            }
            catch (ZeroDiscriminantException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ответ: " + roots.Value.X1);
            }
            catch (NegativeDiscriminantException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IncorrectDataException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (roots.HasValue)
                {
                    Console.WriteLine("Ответы: " + roots.Value.X1 + ", " + roots.Value.X2);

                }

                if (success)
                    Console.WriteLine("Всё завершилось успешно");
                else
                    Console.WriteLine("В работе программы произошла ошибка");
                Console.ReadLine();
            }
        }
    }
}
