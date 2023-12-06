namespace Aoc2023.Days;

public sealed class Day5 : IDay
{
    public int PartOne(string input)
    {
        return (int)Almanac.FromString(input).MinLocationValue;
    }

    public int PartTwo(string input)
    {
        return (int)Almanac.FromString(input).MinLocationValueForRange;
    }

    private readonly struct Almanac(IReadOnlyCollection<long> seeds, IReadOnlyCollection<Mapping> mappings)
    {
        public static Almanac FromString(string str)
        {
            string[] map = str.Split("\r\n\r\n");

            var mappings = map.Skip(1)
                .Select(Mapping.FromString)
                .ToList();

            var seeds = map[0][7..]
                .Split(' ').
                Select(long.Parse)
                .ToList();

            return new Almanac(seeds, mappings);
        }

        public IReadOnlyCollection<SeedRange> RangeOfSeeds =>
            seeds
                .Chunk(2)
                .Select(c => new SeedRange(c[0], c[1]))
                .ToArray();

        private long GetMinLocationValueForRange(
            Mapping currentMapping,
            MappingElement previousMappingElement)
        {
            currentMapping = mappings.Single(c => c.IsFrom(currentMapping.Destination));

            var mappingElements = currentMapping.Elements
                .Select(e => e.FromSeedRangeOrDefault(previousMappingElement.ToDestinationSeedRange()))
                .Where(e => e != default)
                .ToList();

            if (mappingElements.Count < 1)
            {
                throw new InvalidOperationException("Contains no elements");
            }

            var minValue = long.MaxValue;
            if (currentMapping.IsDestinationLocation)
            {
                minValue = mappingElements.Min(m => m.SourceStart);
            }
            else
            {
                foreach (MappingElement mappingElement in mappingElements)
                {
                    long newMinValue = GetMinLocationValueForRange(currentMapping, mappingElement);
                    minValue = Math.Min(newMinValue, minValue);
                }
            }
            return minValue;
        }

        public long MinLocationValueForRange
        {
            get
            {
                var minLocationValue = long.MaxValue;
                foreach (SeedRange seedRange in RangeOfSeeds)
                {
                    Mapping currentMapping = mappings.Single(c => c.IsFrom("seed"));
                    var mappingElements = currentMapping.Elements
                        .Select(e => e.FromSeedRangeOrDefault(seedRange))
                        .Where(e => e != default);

                    foreach (MappingElement mappingElement in mappingElements)
                    {
                        long newValue = GetMinLocationValueForRange(currentMapping, mappingElement);
                        minLocationValue = Math.Min(minLocationValue, newValue);
                    }
                }

                return minLocationValue;
            }
        }

        public long MinLocationValue => CalculateMinLocationValue();

        private long CalculateMinLocationValue()
        {
            var minLocationValue = long.MaxValue;
            foreach (long seed in seeds)
            {
                long mappedValue = GetLocationForSeed(mappings, seed);
                minLocationValue = Math.Min(minLocationValue, mappedValue);
            }

            return minLocationValue;
        }

        private static long GetLocationForSeed(IReadOnlyCollection<Mapping> mappings, long seed)
        {
            Mapping currentMapping = mappings.Single(c => c.IsFrom("seed"));
            long mappedValue = currentMapping.GetMappedValue(seed);

            while (!currentMapping.IsDestinationLocation)
            {
                currentMapping = mappings.Single(c => c.IsFrom(currentMapping.Destination));
                mappedValue = currentMapping.GetMappedValue(mappedValue);
            }

            return mappedValue;
        }
    }

    internal readonly record struct Mapping(string Source, string Destination, IReadOnlyCollection<MappingElement> Elements)
    {
        public static Mapping FromString(string str)
        {
            string[] lines = str.ToLines();

            string[] mapStrings = lines[0].Split('-');
            string from = mapStrings[0];
            string to = mapStrings[2][..^5];

            var mappingElements = lines
                .Skip(1)
                .Select(MappingElement.FromString)
                .OrderBy(m => m.SourceStart)
                .ToList();

            // add missing map elements so we have a mapping for the full range
            var missingMappingElements = new List<MappingElement>();
            if (mappingElements[0].SourceStart != 0)
            {
                missingMappingElements.Add(new MappingElement(0, 0, mappingElements[0].SourceStart));
            }

            for (var i = 0; i < mappingElements.Count - 1; ++i)
            {
                var mapOne = mappingElements[i];
                var mapTwo = mappingElements[i + 1];

                if (mapOne.SourceStart + mapOne.Count != mapTwo.SourceStart)
                {
                    long start = mapOne.SourceStart + mapOne.Count;
                    long count = mapTwo.SourceStart - start;
                    missingMappingElements.Add(new MappingElement(start, start, count));
                }
            }

            long endOfLastMappingElement = mappingElements[^1].SourceStart + mappingElements[^1].Count - 1;
            if (endOfLastMappingElement != long.MaxValue && mappingElements.Any())
            {
                missingMappingElements.Add(new MappingElement(
                    endOfLastMappingElement,
                    endOfLastMappingElement,
                    long.MaxValue - endOfLastMappingElement - 1));
            }

            mappingElements.AddRange(missingMappingElements);

            return new Mapping(from, to, mappingElements);
        }

        public bool IsFrom(string from) => from == Source;

        public bool IsDestinationLocation => Destination == "location";

        public long GetMappedValue(long currentValue)
        {
            return Elements.Single(e => e.IsMappedBy(currentValue)).GetMappedValue(currentValue);
        }
    }

    internal readonly record struct MappingElement(
        long SourceStart,
        long DestinationStart,
        long Count)
    {
        public static MappingElement FromString(string str)
        {
            long[] tokens = str.Split(' ').Select(long.Parse).ToArray();
            return new MappingElement(tokens[1], tokens[0], tokens[2]);
        }

        public MappingElement FromSeedRangeOrDefault(SeedRange seedRange)
        {
            // If ranges don't overlap, early exit
            if (seedRange.Start >= SourceStart + Count ||
                SourceStart >= seedRange.Start + seedRange.Count)
            {
                 return default;
            }

            long endSource = Math.Min(SourceStart + Count, seedRange.Start + seedRange.Count);
            long startSource = Math.Max(SourceStart, seedRange.Start);
            long count = endSource - startSource;
            unchecked
            {
                long startDestination = DestinationStart - (SourceStart - startSource);
                return new MappingElement(startSource, startDestination, count);
            }
        }

        public SeedRange ToDestinationSeedRange()
        {
            return new SeedRange(DestinationStart, Count);
        }
        public bool IsMappedBy(long input)
        {
            return input >= SourceStart && input < (long)SourceStart + (long)Count;
        }

        public long GetMappedValue(long currentValue)
        {
            unchecked
            {
                return currentValue + (DestinationStart - SourceStart);
            }
        }
    }

    internal readonly record struct SeedRange(long Start, long Count);
}