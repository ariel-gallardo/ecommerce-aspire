using Common.Domain.Enums;

namespace Common.Domain.ValueObjects
{
    public class Quantity : IComparable<Quantity>
    {
        public decimal Value { get; set; }
        public Unit Unit { get; set; }

        public int CompareTo(Quantity? other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (IsMonetary(Unit) || IsMonetary(other.Unit))
                throw new InvalidOperationException("Monetary units cannot be compared.");

            if (!AreCompatible(Unit, other.Unit))
                throw new InvalidOperationException($"Incompatible units: {Unit} and {other.Unit}.");

            decimal thisValue = ConvertToBaseUnit(Value, Unit);
            decimal otherValue = ConvertToBaseUnit(other.Value, other.Unit);

            return thisValue.CompareTo(otherValue);
        }

        public static bool operator >(Quantity a, Quantity b) => a.CompareTo(b) > 0;
        public static bool operator <(Quantity a, Quantity b) => a.CompareTo(b) < 0;
        public static bool operator >=(Quantity a, Quantity b) => a.CompareTo(b) >= 0;
        public static bool operator <=(Quantity a, Quantity b) => a.CompareTo(b) <= 0;
        public static bool operator ==(Quantity? a, Quantity? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.CompareTo(b) == 0;
        }
        public static bool operator !=(Quantity? a, Quantity? b) => !(a == b);

        public override bool Equals(object? obj)
        {
            if (obj is not Quantity other) return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConvertToBaseUnit(Value, Unit), GetUnitGroup(Unit));
        }

        private static bool AreCompatible(Unit a, Unit b) =>
            GetUnitGroup(a) == GetUnitGroup(b);

        private static string GetUnitGroup(Unit unit) => unit switch
        {
            Unit.Gram or Unit.Kilogram => "Weight",
            Unit.Liter or Unit.Mililiter => "Volume",
            Unit.Unit => "Count",
            Unit.USD or Unit.ARS => "Money",
            _ => "Unknown"
        };

        private static bool IsMonetary(Unit unit) =>
            unit is Unit.USD or Unit.ARS;

        private static decimal ConvertToBaseUnit(decimal value, Unit unit) => unit switch
        {
            Unit.Kilogram => value * 1000m,
            Unit.Gram => value,
            Unit.Liter => value * 1000m,
            Unit.Mililiter => value,
            Unit.Unit => value,
            _ => throw new InvalidOperationException($"Cannot convert unit {unit}.")
        };

        public override string ToString() => $"{Value} {Unit}";
    }
}
