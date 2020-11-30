using System;
using System.Collections.Generic;

namespace Lab4SAiIO
{
    class Program
    {
        static void Main()
        {
            const int resourceCount = 6;
            var processList = new List<Func<int, double>>
            {
                x =>
                {
                    x++;
                    if (x < 1 || x > 6)
                    {
                        return 0;
                    }

                    if (x < 4)
                    {
                        return (x * x / 2.0) + (x * 2.0);
                    }

                    return x + 6.0;
                },
                x =>
                {
                    x++;
                    if (x < 1 || x > 6)
                    {
                        return 0;
                    }
                    return (x * x / 4.0) + (x / 2.0);
                },
                x =>
                {
                    x++;
                    switch (x)
                    {
                        case 2:
                            return 4.0;
                        case 3:
                        case 4:
                            return 5.0;
                        case 5:
                            return 9.0;
                        case 6:
                            return 10.0;
                        default:
                            return 0.0;
                    }
                },
                x =>
                {
                    x++;
                    if (x < 1 || x > 6)
                    {
                        return 0;
                    } 
                    return x / 2.0;
                }
            };
            var belmanMatrix = new double[processList.Count, resourceCount];
            var belmanMatrixCurrentUsage = new int[processList.Count, resourceCount];
            for (int processIndex = 0; processIndex < processList.Count; processIndex++)
            {
                for (int resourceUsage = 0; resourceUsage < resourceCount; resourceUsage++)
                {
                    if (processIndex == 0)
                    {
                        belmanMatrix[processIndex, resourceUsage] = processList[processIndex](resourceUsage);
                        belmanMatrixCurrentUsage[processIndex, resourceUsage] = resourceUsage + 1;
                    }
                    else
                    {
                        int max = 0;
                        for (int resourceUsageCurrent = 0;
                            resourceUsageCurrent <= resourceUsage;
                            resourceUsageCurrent++)
                        {
                            if (processList[processIndex](resourceUsageCurrent) +
                                belmanMatrix[processIndex - 1, resourceUsage - resourceUsageCurrent] >
                                processList[processIndex](max) + belmanMatrix[processIndex - 1, resourceUsage - max])
                            {
                                max = resourceUsageCurrent;
                            }
                        }
                        belmanMatrix[processIndex, resourceUsage] = processList[processIndex](max) +
                                                                    belmanMatrix[processIndex - 1, resourceUsage - max];
                        belmanMatrixCurrentUsage[processIndex, resourceUsage] = resourceUsage - max;
                    }
                }
            }
            Console.WriteLine("Belman Matrix:");
            for (int indexFirst = 0; indexFirst < processList.Count; indexFirst++)
            {
                for (int indexSecond = 0; indexSecond < resourceCount; indexSecond++)
                {
                    Console.Write(" " + belmanMatrix[indexFirst, indexSecond]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Belman Usage resources Matrix:");
            for (int indexFirst = 0; indexFirst < processList.Count; indexFirst++)
            {
                for (int indexSecond = 0; indexSecond < resourceCount; indexSecond++)
                {
                    Console.Write(" " + belmanMatrixCurrentUsage[indexFirst, indexSecond]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Optimal value:" + belmanMatrix[processList.Count - 1, resourceCount - 1]);
            int currentUsage = resourceCount - 1;
            for (int index = processList.Count - 1; index >= 0; index--)
            {
                Console.WriteLine("Optimal count of resouces for process " + (index + 1) + " is " +
                                  (belmanMatrixCurrentUsage[index, currentUsage]));
                currentUsage -= belmanMatrixCurrentUsage[index, currentUsage];
            }
        }
    }
}