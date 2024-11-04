# .NET Utils Core

This project contains common utilities that might come in handy in all .NET projects.

## Contents

The project contains the following:

* Comparers
    * Comparer that uses predefined sort order
* Extensions
    * `IDictionary`
        * Method for comparing equality of two dictionaries
    * `double`
        * Methods for comparing `double` values (with tolerance)
    * `IEnumerable`
        * Method for adding an index to collection items
        * Methods for combining collections (useful in `TestCaseSource` attribute)
    * Nullable objects
        * Methods for comparing equality of two nullable objects
        * Method for safe-casting object to the given type
* Hierarchy
    * Means to traverse hierarchy of objects
        * Interface for marking the traversable objects
        * Extension methods for the hierarchy traversal
* Math
    * Intervals
        * Interface representing a standard real interval
            * Bounds, inclusivity, length
        * Methods (static) for defining intervals
            * `Open`, `OpenClosed`, `ClosedOpen`, `Closed`
        * Methods (both static and instance) for checking interval properties
            * `Contains`, `IsEqualTo`, `IntersectsWith`
        * Methods (both static and instance) for combining intervals
            * Union, intersection, shortening
    * Interval sets
        * Interface representing a set of standard real intervals
            * Sorted reduced collection of intervals
        * Methods (static) for defining interval sets
            * `Create`, `GetExtendedBy`
        * Methods (both static and instance) for checking interval set properties
            * `Contains`, `IsEqualTo`
        * Methods (both static and instance) for combining interval sets
            * Union, intersection, shortening, complement
