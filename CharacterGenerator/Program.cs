 
using System;

enum RoomType
{
    Empty,
    Enemy,
    StrongEnemy,
    Trap,
    Treasure,
    Rest,
    Merchant
}

class Program
{
    static Random random = new Random();

    // ============================================================
    //                    ТОЧКА ВХОДА
    // ============================================================
    static void Main()
    {
        ShowTitle();

        CreateCharacter(out string name, out string clas,
                        out int health, out int maxHealth,
                        out int attack, out int defense,
                        out int potions);

        double enemyMultiplier = SelectDifficulty();

        int potionsUsed = 0;
        int damageDealt = 0;
        int damageTaken = 0;
        int defeatedEnemies = 0;

        bool isAlive = RunDungeon(name,
                                  ref health, maxHealth,
                                  attack, defense,
                                  ref potions, ref potionsUsed,
                                  ref damageDealt, ref damageTaken,
                                  ref defeatedEnemies,
                                  enemyMultiplier,
                                  out int roomsPassed);

        bool leftDungeon = isAlive;

        ShowStatistics(roomsPassed, defeatedEnemies, potionsUsed, damageDealt, damageTaken);
        FinishGame(isAlive, leftDungeon);
    }

    // ============================================================
    //                    ОБЩИЕ МЕТОДЫ
    // ============================================================
    static void ShowTitle()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       ПОДЗЕМЕЛЬЕ: ПУТЬ ГЕРОЯ");
        Console.WriteLine("========================================");
    }

    // Безопасный ввод целого числа в диапазоне [min, max]
    static int ReadIntInRange(int min, int max, string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write(prompt);

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int value))
            {
                Console.WriteLine($"Ошибка. Введите число от {min} до {max}.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Ошибка. Введите число от {min} до {max}.");
                continue;
            }

            return value;
        }
    }

    // ============================================================
    //                    СОЗДАНИЕ ПЕРСОНАЖА
    // ============================================================
    static void CreateCharacter(out string name, out string clas,
                                out int health, out int maxHealth,
                                out int attack, out int defense,
                                out int potions)
    {
        Console.Write("Введите имя персонажа: ");
        name = Console.ReadLine();

        int classChoice = ReadIntInRange(1, 3,
            "=== ВЫБОР КЛАССА ===\n" +
            "1 — Воин\n" +
            "2 — Маг\n" +
            "3 — Разбойник\n" +
            "Ваш выбор: ");

        switch (classChoice)
        {
            case 1:
                clas = "Воин";
                health = 120; maxHealth = 120;
                attack = 18; defense = 10; potions = 3;
                break;
            case 2:
                clas = "Маг";
                health = 80; maxHealth = 80;
                attack = 25; defense = 4; potions = 5;
                break;
            default:
                clas = "Разбойник";
                health = 100; maxHealth = 100;
                attack = 21; defense = 6; potions = 3;
                break;
        }

        ShowCharacter(name, clas, health, maxHealth, attack, defense, potions);
    }

    static void ShowCharacter(string name, string clas,
                              int health, int maxHealth,
                              int attack, int defense, int potions)
    {
        Console.WriteLine();
        Console.WriteLine("============================");
        Console.WriteLine("    Созданный персонаж");
        Console.WriteLine("============================");
        Console.WriteLine();
        Console.WriteLine($"Имя = {name}");
        Console.WriteLine($"Класс = {clas}");
        Console.WriteLine();
        Console.WriteLine("-----Характеристики-----");
        Console.WriteLine($"Здоровье = {health}/{maxHealth}");
        Console.WriteLine($"Атака = {attack}");
        Console.WriteLine($"Защита = {defense}");
        Console.WriteLine($"Зелья = {potions}");
    }

    static double SelectDifficulty()
    {
        int difficulty = ReadIntInRange(1, 3,
            "Выберите сложность подземелья:\n" +
            "1 — Лёгкий\n" +
            "2 — Обычный\n" +
            "3 — Сложный\n" +
            "Ваш выбор: ");

        if (difficulty == 1)
        {
            Console.WriteLine("Вы выбрали лёгкую сложность.");
            return 0.8;
        }
        if (difficulty == 2)
        {
            Console.WriteLine("Вы выбрали обычную сложность.");
            return 1.0;
        }

        Console.WriteLine("Вы выбрали сложную сложность.");
        return 1.3;
    }

    // ============================================================
    //                    ИГРОВЫЕ ФОРМУЛЫ
    // ============================================================
    static int CalculateDamage(int attack, int defense)
    {
        int damage = attack - defense;
        return damage < 1 ? 1 : damage;
    }

    static int RestoreHealth(int health, int maxHealth, int amount)
    {
        int newHealth = health + amount;
        return newHealth > maxHealth ? maxHealth : newHealth;
    }

    // ============================================================
    //                    ПРОХОЖДЕНИЕ ПОДЗЕМЕЛЬЯ
    // ============================================================
    static RoomType GenerateRoom()
    {
        int r = random.Next(1, 101);

        if (r <= 20) return RoomType.Empty;
        if (r <= 45) return RoomType.Enemy;
        if (r <= 60) return RoomType.StrongEnemy;
        if (r <= 75) return RoomType.Trap;
        if (r <= 87) return RoomType.Treasure;
        if (r <= 95) return RoomType.Rest;
        return RoomType.Merchant;
    }

    // Возвращает true, если игрок жив (победил или вышел), false - если погиб
    static bool RunDungeon(string playerName,
                           ref int health, int maxHealth,
                           int attack, int defense,
                           ref int potions, ref int potionsUsed,
                           ref int damageDealt, ref int damageTaken,
                           ref int defeatedEnemies,
                           double enemyMultiplier,
                           out int roomsPassed)
    {
        roomsPassed = 0;
        int attackBonus = 0;

        for (int room = 1; room <= 5; room++)
        {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine($"КОМНАТА {room} ИЗ 5");
            Console.WriteLine("========================================");

            int choice = ReadIntInRange(1, 2,
                "1 — Продолжить\n" +
                "2 — Покинуть подземелье\n" +
                "Ваш выбор: ");

            if (choice == 2)
            {
                Console.WriteLine("Вы покинули подземелье.");
                return true;
            }

            roomsPassed++;

            RoomType roomType = GenerateRoom();
            Console.WriteLine("Вы открываете дверь...");

            bool alive = ProcessRoom(roomType, playerName,
                                     ref health, maxHealth,
                                     attack, defense,
                                     ref potions, ref potionsUsed,
                                     ref damageDealt, ref damageTaken,
                                     ref defeatedEnemies,
                                     ref attackBonus,
                                     enemyMultiplier);

            if (!alive)
                return false;
        }

        // Финальный босс
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("             ФИНАЛЬНЫЙ БОСС");
        Console.WriteLine("========================================");
        Console.WriteLine("В глубине подземелья появляется Хранитель Тьмы!");

        int bossHealth = (int)Math.Round(150 * enemyMultiplier);
        int bossAttack = (int)Math.Round(20 * enemyMultiplier);
        int totalAttack = attack + attackBonus;

        bool won = StartBattle(playerName, totalAttack, defense,
                               ref health, maxHealth,
                               ref potions, ref potionsUsed,
                               ref damageDealt, ref damageTaken,
                               "Хранитель Тьмы", bossHealth, bossAttack);

        if (won && health > 0)
        {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("                ПОБЕДА!");
            Console.WriteLine("========================================");
            Console.WriteLine("Вы победили Хранителя Тьмы");
            Console.WriteLine("и выбрались из подземелья.");
            Console.WriteLine("Поздравляем!");
            return true;
        }

        return false;
    }

    // Возвращает false, если игрок погиб
    static bool ProcessRoom(RoomType roomType, string playerName,
                            ref int health, int maxHealth,
                            int attack, int defense,
                            ref int potions, ref int potionsUsed,
                            ref int damageDealt, ref int damageTaken,
                            ref int defeatedEnemies,
                            ref int attackBonus,
                            double enemyMultiplier)
    {
        switch (roomType)
        {
            case RoomType.Empty:
                Console.WriteLine("Комната пуста. Можно идти дальше.");
                return true;

            case RoomType.Enemy:
            case RoomType.StrongEnemy:
            {
                string enemyName;
                int enemyBaseHealth;
                int enemyBaseAttack;

                int enemyType = random.Next(1, 4);
                if (enemyType == 1)
                {
                    enemyName = "Гоблин";
                    enemyBaseHealth = 40;
                    enemyBaseAttack = 8;
                }
                else if (enemyType == 2)
                {
                    enemyName = "Орк";
                    enemyBaseHealth = 70;
                    enemyBaseAttack = 12;
                }
                else
                {
                    enemyName = "Рыцарь";
                    enemyBaseHealth = 100;
                    enemyBaseAttack = 16;
                }

                int enemyHealth = (int)Math.Round(enemyBaseHealth * enemyMultiplier);
                int enemyAttack = (int)Math.Round(enemyBaseAttack * enemyMultiplier);

                if (roomType == RoomType.StrongEnemy)
                {
                    enemyHealth = (int)Math.Round(enemyHealth * 1.25);
                    enemyAttack = (int)Math.Round(enemyAttack * 1.25);
                    Console.WriteLine("Это сильный противник!");
                }

                Console.WriteLine($"На вас напал: {enemyName}!");
                Console.WriteLine($"Здоровье врага: {enemyHealth}");
                Console.WriteLine($"Атака врага: {enemyAttack}");

                int totalAttack = attack + attackBonus;
                bool won = StartBattle(playerName, totalAttack, defense,
                                       ref health, maxHealth,
                                       ref potions, ref potionsUsed,
                                       ref damageDealt, ref damageTaken,
                                       enemyName, enemyHealth, enemyAttack);

                if (won && health > 0)
                {
                    defeatedEnemies++;
                    Console.WriteLine("Бой окончен. Вы продолжаете путь.");
                }

                return health > 0;
            }

            case RoomType.Trap:
            {
                bool alive = true;
                ProcessTrap(ref health, maxHealth, ref damageTaken, ref alive);
                return alive;
            }

            case RoomType.Treasure:
                ProcessTreasure(ref potions, ref attackBonus);
                return true;

            case RoomType.Rest:
                ProcessRest(ref health, maxHealth);
                return true;

            case RoomType.Merchant:
                ProcessMerchant(ref potions, ref attackBonus);
                return true;
        }

        return true;
    }

    // ============================================================
    //                    ОБРАБОТКА КОМНАТ
    // ============================================================
    static void ProcessTrap(ref int health, int maxHealth,
                            ref int damageTaken, ref bool isAlive)
    {
        Console.WriteLine("Вы попали в ловушку!");
        int trapDamage = 15;
        damageTaken += trapDamage;
        health -= trapDamage;

        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            Console.WriteLine("Ловушка оказалась смертельной!");
        }
        else
        {
            Console.WriteLine($"Получено {trapDamage} урона.");
            Console.WriteLine($"Ваше здоровье: {health}/{maxHealth}");
        }
    }

    static void ProcessTreasure(ref int potions, ref int attackBonus)
    {
        Console.WriteLine("Вы нашли сундук!");
        int reward = random.Next(1, 4);

        switch (reward)
        {
            case 1:
                potions++;
                Console.WriteLine($"Награда: зелье! Теперь зелий: {potions}");
                break;
            case 2:
                attackBonus += 2;
                Console.WriteLine($"Награда: золото! Бонус к атаке: +2 (всего +{attackBonus})");
                break;
            default:
                attackBonus += 4;
                Console.WriteLine($"Награда: древний артефакт! Бонус к атаке: +4 (всего +{attackBonus})");
                break;
        }
    }

    static void ProcessRest(ref int health, int maxHealth)
    {
        Console.WriteLine("Вы нашли безопасное место для отдыха.");
        int before = health;
        health = RestoreHealth(health, maxHealth, 20);
        Console.WriteLine($"Восстановлено {health - before} здоровья.");
        Console.WriteLine($"Ваше здоровье: {health}/{maxHealth}");
    }

    static void ProcessMerchant(ref int potions, ref int attackBonus)
    {
        Console.WriteLine("В комнате вас встречает торговец.");
        int choice = ReadIntInRange(1, 3,
            "1 — Купить зелье\n" +
            "2 — Купить увеличение атаки\n" +
            "3 — Уйти\n" +
            "Ваш выбор: ");

        switch (choice)
        {
            case 1:
                potions++;
                Console.WriteLine($"Вы купили зелье! Теперь зелий: {potions}");
                break;
            case 2:
                attackBonus += 3;
                Console.WriteLine($"Вы усилили атаку! Бонус к атаке: +3 (всего +{attackBonus})");
                break;
            default:
                Console.WriteLine("Вы ушли от торговца.");
                break;
        }
    }

    // ============================================================
    //                    БОЕВАЯ СИСТЕМА
    // ============================================================
    // Возвращает true, если игрок победил; false - если проиграл или сбежал
    static bool StartBattle(string playerName, int attack, int defense,
                            ref int health, int maxHealth,
                            ref int potions, ref int potionsUsed,
                            ref int damageDealt, ref int damageTaken,
                            string enemyName, int enemyHealth, int enemyAttack)
    {
        while (health > 0 && enemyHealth > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"Ваше здоровье: {health} / {maxHealth}");
            Console.WriteLine($"{enemyName}: {enemyHealth} HP");
            Console.WriteLine($"Зелий: {potions}");

            bool escaped = PlayerTurn(playerName, attack, defense,
                                      ref health, maxHealth,
                                      ref potions, ref potionsUsed,
                                      ref damageDealt,
                                      enemyName, ref enemyHealth,
                                      out bool defended);

            if (escaped)
                return false;

            if (enemyHealth <= 0)
                return true;

            if (EnemyTurn(enemyName, enemyAttack, defense, defended,
                          ref health, maxHealth, ref damageTaken))
                return false;
        }

        return enemyHealth <= 0 && health > 0;
    }

    // Возвращает true, если игрок сбежал.
    // out defended - защищался ли игрок в этот ход.
    static bool PlayerTurn(string playerName, int attack, int defense,
                           ref int health, int maxHealth,
                           ref int potions, ref int potionsUsed,
                           ref int damageDealt,
                           string enemyName, ref int enemyHealth,
                           out bool defended)
    {
        defended = false;

        int action = ReadIntInRange(1, 4,
            "1 — Атаковать\n" +
            "2 — Использовать зелье\n" +
            "3 — Защищаться\n" +
            "4 — Сбежать\n" +
            "Ваш выбор: ");

        switch (action)
        {
            case 1:
            {
                int dmg = attack;

                if (random.Next(1, 101) <= 20)
                {
                    dmg *= 2;
                    Console.WriteLine("КРИТИЧЕСКИЙ УДАР!");
                }

                if (dmg > enemyHealth)
                    dmg = enemyHealth;

                enemyHealth -= dmg;
                damageDealt += dmg;

                Console.WriteLine($"Вы наносите {dmg} урона!");

                if (enemyHealth == 0)
                    Console.WriteLine($"{enemyName} повержен!");

                return false;
            }

            case 2:
                if (potions <= 0)
                {
                    Console.WriteLine("У вас нет зелий!");
                }
                else if (health >= maxHealth)
                {
                    Console.WriteLine("Здоровье уже максимальное!");
                }
                else
                {
                    int before = health;
                    health = RestoreHealth(health, maxHealth, 30);
                    potions--;
                    potionsUsed++;
                    Console.WriteLine("Вы использовали зелье!");
                    Console.WriteLine($"Восстановлено {health - before} здоровья.");
                    Console.WriteLine($"Здоровье: {health}/{maxHealth}");
                }
                return false;

            case 3:
                defended = true;
                Console.WriteLine("Вы приготовились защищаться!");
                return false;

            default: // 4 — сбежать
                Console.WriteLine("Вы сбежали от противника.");
                return true;
        }
    }

    // Возвращает true, если игрок погиб.
    static bool EnemyTurn(string enemyName, int enemyAttack, int playerDefense,
                          bool playerDefended,
                          ref int health, int maxHealth,
                          ref int damageTaken)
    {
        int rawDamage = CalculateDamage(enemyAttack, playerDefense);

        if (playerDefended)
        {
            rawDamage = Math.Max(1, rawDamage / 2);
            Console.WriteLine("Защита уменьшила полученный урон!");
        }

        damageTaken += rawDamage;
        health -= rawDamage;

        if (health < 0)
            health = 0;

        Console.WriteLine($"{enemyName} атакует! Вы получаете {rawDamage} урона.");
        Console.WriteLine($"Ваше здоровье: {health}/{maxHealth}");

        if (health == 0)
        {
            Console.WriteLine("Ваш персонаж погиб!");
            return true;
        }

        return false;
    }

    // ============================================================
    //                    СТАТИСТИКА И ЗАВЕРШЕНИЕ
    // ============================================================
    static void ShowStatistics(int roomsPassed, int defeatedEnemies,
                               int potionsUsed, int damageDealt, int damageTaken)
    {
        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("       СТАТИСТИКА ПРИКЛЮЧЕНИЯ");
        Console.WriteLine("========================================");
        Console.WriteLine($"Пройдено комнат: {roomsPassed}");
        Console.WriteLine($"Побеждено врагов: {defeatedEnemies}");
        Console.WriteLine($"Использовано зелий: {potionsUsed}");
        Console.WriteLine($"Нанесено урона: {damageDealt}");
        Console.WriteLine($"Получено урона: {damageTaken}");
        Console.WriteLine("========================================");
    }

    static void FinishGame(bool isAlive, bool leftDungeon)
    {
        if (!isAlive)
        {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("              ПОРАЖЕНИЕ");
            Console.WriteLine("========================================");
            Console.WriteLine("Ваш персонаж погиб.");
            Console.WriteLine("Подземелье оказалось слишком опасным.");
        }
        else
        {
            Console.WriteLine("Игра завершена.");
        }
    }
}