 
Random random = new Random();

Console.WriteLine("========================================");
Console.WriteLine("       ПОДЗЕМЕЛЬЕ: ПУТЬ ГЕРОЯ");
Console.WriteLine("========================================");
Console.Write("Введите имя персонажа: ");
string name = Console.ReadLine();

int classChoice;
do
{
    Console.WriteLine();
    Console.WriteLine("=== ВЫБОР КЛАССА ===");
    Console.WriteLine("1 — Воин");
    Console.WriteLine("2 — Маг");
    Console.WriteLine("3 — Разбойник");
    Console.Write("Ваш выбор: ");

    string input = Console.ReadLine();

    if (!int.TryParse(input, out classChoice))
    {
        classChoice = 0;
        Console.WriteLine("Ошибка. Введите число от 1 до 3.");
    }
    else if (classChoice < 1 || classChoice > 3)
    {
        Console.WriteLine("Ошибка. Такого класса нет.");
    }

} while (classChoice < 1 || classChoice > 3);

int health = 0;
int maxHealth = 0;
int attack = 0;
int defense = 0;
int potions = 0;
bool isAlive = true;

string clas;
switch (classChoice)
{
    case 1:
        clas = "Воин";
        health = 120;
        maxHealth = 120;
        attack = 18;
        defense = 10;
        potions = 3;
        break;
    case 2:
        clas = "Маг";
        health = 80;
        maxHealth = 80;
        attack = 25;
        defense = 4;
        potions = 5;
        break;
    case 3:
        clas = "Разбойник";
        health = 100;
        maxHealth = 100;
        attack = 21;
        defense = 6;
        potions = 3;
        break;
    default:
        clas = "Неизвестный класс";
        break;
}

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

int difficulty;
do
{
    Console.WriteLine();
    Console.WriteLine("Выберите сложность подземелья:");
    Console.WriteLine("1 — Лёгкий");
    Console.WriteLine("2 — Обычный");
    Console.WriteLine("3 — Сложный");
    Console.Write("Ваш выбор: ");

    string input = Console.ReadLine();

    if (!int.TryParse(input, out difficulty))
    {
        difficulty = 0;
        Console.WriteLine("Введите число от 1 до 3.");
    }
    else if (difficulty < 1 || difficulty > 3)
    {
        Console.WriteLine("Такого уровня сложности нет.");
    }
} while (difficulty < 1 || difficulty > 3);

double enemyMultiplier = 1.0;

if (difficulty == 1)
{
    enemyMultiplier = 0.8;
    Console.WriteLine("Вы выбрали лёгкую сложность.");
}
else if (difficulty == 2)
{
    enemyMultiplier = 1.0;
    Console.WriteLine("Вы выбрали обычную сложность.");
}
else
{
    enemyMultiplier = 1.3;
    Console.WriteLine("Вы выбрали сложную сложность.");
}

// Статистика
int roomsPassed = 0;
int defeatedEnemies = 0;
int potionsUsed = 0;
int damageDealt = 0;
int damageTaken = 0;
bool leftDungeon = false;

// Исследование пяти комнат
for (int room = 1; room <= 5 && isAlive && !leftDungeon; room++)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine($"КОМНАТА {room} ИЗ 5");
    Console.WriteLine("========================================");

    int choice;
    do
    {
        Console.WriteLine();
        Console.WriteLine($"Вы исследуете комнату {room}.");
        Console.WriteLine("1 — Продолжить");
        Console.WriteLine("2 — Покинуть подземелье");
        Console.Write("Ваш выбор: ");

        string input = Console.ReadLine();

        if (!int.TryParse(input, out choice))
        {
            choice = 0;
            Console.WriteLine("Введите число 1 или 2.");
        }
        else if (choice < 1 || choice > 2)
        {
            Console.WriteLine("Такого действия нет.");
        }

    } while (choice < 1 || choice > 2);

    if (choice == 2)
    {
        leftDungeon = true;
        Console.WriteLine("Вы покинули подземелье.");
        break;
    }
    roomsPassed++;

    Console.WriteLine("Вы открываете дверь...");
    int eventType = random.Next(1, 6);

    if (eventType == 1 || eventType == 2)
    {
        string enemyName;
        int enemyBaseHealth;
        int enemyBaseAttack;

        // Выбираем один из трeх типов врагов
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

        // Событие 2 более опасный противник
        if (eventType == 2)
        {
            enemyHealth = (int)Math.Round(enemyHealth * 1.25);
            enemyAttack = (int)Math.Round(enemyAttack * 1.25);
            Console.WriteLine("Это сильный противник!");
        }

        Console.WriteLine($"На вас напал: {enemyName}!");
        Console.WriteLine($"Здоровье врага: {enemyHealth}");
        Console.WriteLine($"Атака врага: {enemyAttack}");

        bool hasEscaped = false;

        //Цикл боя
        while (isAlive && enemyHealth > 0 && !hasEscaped)
        {
            Console.WriteLine();
            Console.WriteLine($"Ваше здоровье: {health} / {maxHealth}");
            Console.WriteLine($"{enemyName}: {enemyHealth} HP");
            Console.WriteLine($"Зелий: {potions}");

            int action;
            do
            {
                Console.WriteLine();
                Console.WriteLine("1 — Атаковать");
                Console.WriteLine("2 — Использовать зелье");
                Console.WriteLine("3 — Защищаться");
                Console.Write("Ваш выбор: ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out action))
                {
                    action = 0;
                    Console.WriteLine("Введите число от 1 до 3.");
                }
                else if (action < 1 || action > 3)
                {
                    Console.WriteLine("Такого действия нет.");
                }

            } while (action < 1 || action > 3);

            bool isDefending = true;
            switch (action)
            {
                case 1:
                    int actualDamage = attack;

                    // Критический удар: вероятность 20%
                    bool isCritical = random.Next(1, 101) <= 20;
                    if (isCritical)
                    {
                        actualDamage *= 2;
                        Console.WriteLine("КРИТИЧЕСКИЙ УДАР!");
                    }

                    // Не засчитываем лишний урон сверх здоровья врага
                    if (actualDamage > enemyHealth)
                    {
                        actualDamage = enemyHealth;
                    }

                    enemyHealth -= actualDamage;
                    damageDealt += actualDamage;
                    Console.WriteLine($"Вы наносите {actualDamage} урона!");

                    if (enemyHealth == 0)
                    {
                        Console.WriteLine($"{enemyName} повержен!");
                    }
                    break;

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
                        health += 30;

                        if (health > maxHealth)
                        {
                            health = maxHealth;
                        }

                        potions--;
                        potionsUsed++;

                        Console.WriteLine("Вы использовали зелье!");
                        Console.WriteLine($"Здоровье: {health} / {maxHealth}");
                    }

                    break;

                case 3:
                    isDefending = true;
                    Console.WriteLine("Вы приготовились защищаться!");
                    break;
                
                case 4:
                    hasEscaped = true;
                    Console.WriteLine("Вы сбежали от противника.");
                    break;
            }
            // Враг отвечает, если он жив и игрок не сбежал
            if (enemyHealth > 0 && !hasEscaped)
            {
                int rawDamage = enemyAttack - defense;
                if (rawDamage < 1)
                {
                    rawDamage = 1;
                }

                if (isDefending)
                {
                    rawDamage = Math.Max(1, rawDamage / 2);
                    Console.WriteLine("Защита уменьшила полученный урон!");
                }

                damageTaken += rawDamage;
                health -= rawDamage;

                if (health < 0)
                {
                    health = 0;
                }

                Console.WriteLine($"{enemyName} атакует! Вы получаете {rawDamage} урона.");
                Console.WriteLine($"Ваше здоровье: {health}/{maxHealth}");

                if (health == 0)
                {
                    isAlive = false;
                    Console.WriteLine("Ваш персонаж погиб!");
                }
            }
        }

        if (enemyHealth <= 0)
        {
            defeatedEnemies++;
        }

        if (isAlive && enemyHealth <= 0)
        {
            Console.WriteLine("Бой окончен. Вы продолжаете путь.");
        }
    }
    else if (eventType == 3)
    {
        Console.WriteLine("Вы нашли зелье!");
        potions++;
        Console.WriteLine($"Теперь у вас зелий: {potions}");
    }
    else if (eventType == 4)
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
            Console.WriteLine($"Ваше здоровье: {health} / {maxHealth}");
        }
    }
    else if (eventType == 6)
    {
        Console.WriteLine("Вы нашли секретный проход!");
        Console.WriteLine("Следующая комната пропускается.");

        room++;
        continue;
    }
    else
    {
        Console.WriteLine("Комната пуста. Можно идти дальше.");
    }

    if (!isAlive)
    {
        break;
    }
}
// Финальный босс 
if (isAlive && !leftDungeon)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("             ФИНАЛЬНЫЙ БОСС");
    Console.WriteLine("========================================");
    Console.WriteLine("В глубине подземелья появляется Хранитель Тьмы!");

    string bossName = "Хранитель Тьмы";
    int bossHealth = (int)Math.Round(150 * enemyMultiplier);
    int bossAttack = (int)Math.Round(20 * enemyMultiplier);
    bool hasEscaped = false;

    Console.WriteLine($"HP: {bossHealth}");
    Console.WriteLine($"Атака: {bossAttack}");

    while (isAlive && bossHealth > 0 && !hasEscaped)
    {
        Console.WriteLine();
        Console.WriteLine($"{name}: {health}/{maxHealth} HP");
        Console.WriteLine($"{bossName}: {bossHealth} HP");
        Console.WriteLine($"Зелья: {potions}");

        int action;
        do
        {
            Console.WriteLine();
            Console.WriteLine("1 — Атаковать");
            Console.WriteLine("2 — Использовать зелье");
            Console.WriteLine("3 — Защищаться");
            Console.WriteLine("4 — Сбежать");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out action))
            {
                action = 0;
                Console.WriteLine("Введите число от 1 до 4.");
            }
            else if (action < 1 || action > 4)
            {
                Console.WriteLine("Такого действия нет.");
            }
        } while (action < 1 || action > 4);

        bool isDefending = false;

        switch (action)
        {
            case 1:
                int actualDamage = attack;
                bool isCritical = random.Next(1, 101) <= 20;

                if (isCritical)
                {
                    actualDamage *= 2;
                    Console.WriteLine("КРИТИЧЕСКИЙ УДАР!");
                }

                if (actualDamage > bossHealth)
                {
                    actualDamage = bossHealth;
                }

                bossHealth -= actualDamage;
                damageDealt += actualDamage;
                Console.WriteLine($"Вы наносите {actualDamage} урона!");

                if (bossHealth == 0)
                {
                    Console.WriteLine($"{bossName} повержен!");
                }
                break;

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
                    int oldHealth = health;
                    health += 30;
                    if (health > maxHealth)
                    {
                        health = maxHealth;
                    }

                    potions--;
                    potionsUsed++;
                    Console.WriteLine($"Вы восстановили {health - oldHealth} здоровья.");
                    Console.WriteLine($"Зелий осталось: {potions}");
                }
                break;

            case 3:
                isDefending = true;
                Console.WriteLine("Вы приготовились защищаться!");
                break;

            case 4:
                hasEscaped = true;
                leftDungeon = true;
                Console.WriteLine("Вы сбежали от финального босса.");
                break;
        }

        if (bossHealth > 0 && !hasEscaped)
        {
            int rawDamage = bossAttack - defense;
            if (rawDamage < 1)
            {
                rawDamage = 1;
            }

            if (isDefending)
            {
                rawDamage = Math.Max(1, rawDamage / 2);
                Console.WriteLine("Защита уменьшила полученный урон!");
            }

            damageTaken += rawDamage;
            health -= rawDamage;

            if (health < 0)
            {
                health = 0;
            }

            Console.WriteLine($"{bossName} атакует! Вы получаете {rawDamage} урона.");
            Console.WriteLine($"Ваше здоровье: {health}/{maxHealth}");

            if (health == 0)
            {
                isAlive = false;
                Console.WriteLine("Ваш персонаж погиб!");
            }
        }
    }
    if (isAlive && bossHealth <= 0)
    {
       Console.WriteLine();
       Console.WriteLine("========================================");
       Console.WriteLine("                ПОБЕДА!");
       Console.WriteLine("========================================");
       Console.WriteLine("Вы победили Хранителя Тьмы");
       Console.WriteLine("и выбрались из подземелья.");
       Console.WriteLine("Поздравляем!");
    }
}
if (!isAlive)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("              ПОРАЖЕНИЕ");
    Console.WriteLine("========================================");
    Console.WriteLine("Ваш персонаж погиб.");
    Console.WriteLine("Подземелье оказалось слишком опасным.");
}
else if (leftDungeon)
{
    Console.WriteLine("Игра завершена.");
}

// Итоговая статистика
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
















