# Басенко Кирилл 953505

## Что это?

Это Windows-служба со времен 2й лабораторной: мониторинг, архивация, перемещение etc. Время идет — прогресс не стоит — лаба развивается и становится более гибкой.

## Чему же она научилась?

Прежде всего, покидать зону комфорта, прыгать по директориям. Обратная сторона медали — по каким? Если раньше она даже **до** рождения знала, каков путь, то сейчас ей предстоит получить направление. Для этого ей пришлось научиться читать **config-файлы**, в которых лежат указания. Как? Она обзавелась такими замечательными друзьями, как **парсеры**, которые, ведая ей содержание конфигов, помогают ей ориентироваться в директориях.

## Если с другом вышел в путь...

С чего же начинается путешеcтвие? С подготовки **overseer**'а.
**Provider**, подобно медиуму, вещает указания/направления парсеров.

```C#

protected override void OnStart(string[] args)
{
  var provider = new Provider(new Parser());
  var configPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, configFileName);

  ...

  Options options = provider.GetConfig<Options>(configPath);
  overseer = new Overseer(options);
}

```

## Что внутри?

**Provider** обращается к парсерам, преподав им путь к кофигу.

```C#

class Provider
{
		private readonly IParser _configParser;

		public T GetConfig<T>(string filePath)
		{
			return _configParser.Parse<T>(filePath);
		}
    
    ...
}

```

Ну, на самом деле он обращается к "парсеру-старшему", который выбирает лучшего из лучших, он выбирает достойного.

```C#

public T Parse<T>(string filePath)
{
  ...
  switch (System.IO.Path.GetExtension(filePath))
  {
    case (".json"):
    {
      return _jsonParser.Parse<T>(filePath);
    }
    case (".xml"):
    {
      return _xmlParser.Parse<T>(filePath);
    }
  }
  ...
}

```

## Что за магия происходит дальше?

Если кратко:
1. Создается сущность требуемого типа.
2. Парсер ходит по узлам конфига и ищет поля/свойства требуемого типа с помощью рефлексии.
	- 2.1. Если нашли, то:
		- 2.1.1. Если узел представляет собой некий вложенный класс, то этот вложенный класс парситься так же (начиная с пунтка 1).
		- 2.1.2. Если узел не является вложенным классом, то читается содержимое узла и значение помещается в поле/свойство нашей сущности.
	- 2.2. Если не нашли, то ничего страшного, идем дальше.
3. Пройдя по всем узлам, и, подобравши все возможные значения, возвращаем сущность.

Более глубокие/подробные знания о работе предоставленных парсеров — эзотерика. С этими запрещенными алгоритмами можно ознакомиться в файлах ***JsonParser.cs*** и ***XmlParser.cs*** в папке **Parsers**.

## "Прости, парсеры в сделку не входили"

У каджого своя цель... Целью парсеров было предоставление направления. Целью провайдера было передача того самого направления. Заимев координаты, нашему **overseer**'y предстоит выполнить свой долг. По-английски покинув метод OnStart(...), overseer, оставив парсеры и провайдера дожидаться в саспенсе GC, с хладнокровностью машины, забирает все нужное из **options**, все, что ему надобно. Кидая объедки, как кости, **slave**'у.

```C#

public Overseer(Options options)
{
  sourceDirectoryPath = options.SourceDirectoryPath;
  logPath = options.LogPath;

  slave = new Commands(options);

  watcher = new FileSystemWatcher(sourceDirectoryPath);
            
  ...
  watcher.Created += slave.OnAdded;
  ...
}

```

**slave** — тот парень, который делает всю грязную работу (архивирование, шифрование, перемещение etc).

## Все новое — хорошо забытое старое

Последующее описание — описание 2й лабы. Поэтому на этом конец. Спасибо :)
