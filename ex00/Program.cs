using System;

double sum = 0;
double rate = 0;
int term = 0;
int selectedMonth = 0; 
double payment = 0;

double overpayment = 0;
double overpayment1 = 0;
double overpaystart = 0;
double sumofdept = 0;
double perspayment = 0;
double persentinMonth = 0;
double annuitet = 0;
double annuitet_t;
const bool calculationDecriseSum = true;
const bool calculationDecrisePeriod = false;

bool sum_b;
bool rate_b;
bool term_b;
bool selectedMonth_b;
bool payment_b;

DateTime currentdate = DateTime.Now.AddMonths(1);
currentdate = new DateTime(currentdate.Year, currentdate.Month, 1);
DateTime currentdate_t = currentdate;

if (args.Length != 5)
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
    return;
}

sum_b = Double.TryParse(args[0], out sum);
rate_b = Double.TryParse(args[1], out rate);
term_b = Int32.TryParse(args[2], out term);
selectedMonth_b = Int32.TryParse(args[3], out selectedMonth);
payment_b = Double.TryParse(args[4], out payment);

if (sum_b == false || rate_b == false || term_b == false || selectedMonth_b == false || payment_b == false 
    || sum < 0 || rate < 0 || term < 0 || selectedMonth > term || selectedMonth < 0 || payment < 0)
{
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
    return;
}

int DaysInYear(DateTime datenow)
{
    if (DateTime.IsLeapYear(datenow.Year) == true)
        return (366);
    else
        return (365);
}

sumofdept = sum;
double SummInRubles(double sumofdepttemp, DateTime currmonth)
{
    DateTime lastmonth = currmonth.AddMonths(-1);
    
    perspayment = (sumofdepttemp * rate * (DateTime.DaysInMonth(lastmonth.Year, lastmonth.Month))) / (100 * DaysInYear(lastmonth));
    return (perspayment);
}

string FormatMoney(double money)
{
    return ($"{money:F2} p.");
}

persentinMonth = rate / 12 / 100;
annuitet = (sum * persentinMonth * Math.Pow((1 + persentinMonth), term))/((Math.Pow(1 + persentinMonth, term)) - 1);
annuitet_t = annuitet;

for (int i = 0; i < term; i++)
{
    double sumperscurr = SummInRubles(sumofdept, currentdate_t);
    sumofdept = sumofdept - annuitet_t + sumperscurr;
    overpaystart += sumperscurr;
    currentdate_t = currentdate_t.AddMonths(1);
}

double OverPayment(bool flag, int term_t, double annuitet_t)
{
    DateTime currentdate_t = currentdate;
    double overpayment_t = 0;
    double sumperscurr_t = 0;
    double sumofdept_t = sum;
    
    Console.WriteLine("{0,-15} {1,-20} {2,-35} {3,-25} {4,-15} {5,-15}","Номер месяца", "Дата платежа", "Общая сумма платежа (Платеж)", "Тело кредита (ОД)", "Проценты", "Остаток долга");
    Console.WriteLine("{0,-15} {1,-20} {2,-35} {3,-25} {4,-15} {5,-15}", "", "", FormatMoney(annuitet_t), FormatMoney(sum), FormatMoney(overpaystart), FormatMoney(sum));
    
    for (int i = 0; i < term_t; i++)
    {
        sumperscurr_t = SummInRubles(sumofdept_t, currentdate_t);
        sumofdept_t = sumofdept_t - annuitet_t + sumperscurr_t;
        overpayment_t += sumperscurr_t;
        if (sumofdept_t > 0)
        {
            if (i == selectedMonth - 1)
            {
                sumofdept_t = sumofdept_t - payment;
                if (flag == calculationDecriseSum)
                    annuitet_t =
                        (sumofdept_t * persentinMonth * Math.Pow((1 + persentinMonth), term_t - selectedMonth)) /
                        ((Math.Pow(1 + persentinMonth, term_t - selectedMonth)) - 1);
                else
                    term_t = (int) Math.Ceiling(Math.Log(annuitet_t / (annuitet_t - persentinMonth * sumofdept_t), persentinMonth + 1)) + i + 1;
            }
        }
        
        if (sumofdept_t < 0)
            sumofdept_t = 0;

        Console.WriteLine("{0,-15} {1,-20:dd.MM.yyyy} {2,-35} {3,-25} {4,-15} {5,-15}", i + 1, currentdate_t, FormatMoney(annuitet_t), FormatMoney(annuitet_t - sumperscurr_t), FormatMoney(sumperscurr_t), FormatMoney(sumofdept_t));
        currentdate_t = currentdate_t.AddMonths(1);
    }
    if (flag == calculationDecriseSum)
        Console.WriteLine("Переплата при уменьшении платежа: {0:F2} р.", overpayment_t);
    else
        Console.WriteLine("Переплата при уменьшении срока: {0:F2} р.", overpayment_t);
    return (overpayment_t);
}

overpayment1 = OverPayment(calculationDecriseSum, term, annuitet);
overpayment = OverPayment(calculationDecrisePeriod, term, annuitet);

if (overpayment1 > overpayment)
    Console.WriteLine("Уменьшение срока выгоднее уменьшения платежа на {0:F2} р.", overpayment1 - overpayment);
else if (overpayment1 < overpayment)
    Console.WriteLine("Уменьшение платежа выгоднее уменьшения срока на {0:F2} р.", overpayment - overpayment1);
else
    Console.WriteLine("Переплата одинакова в обоих вариантах.");