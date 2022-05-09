# Entity References

To give the ability to reference things across scenes you have to add the Entity component to the GameObject you wish to be able to reference.

The Entity componet assigns the GameObject a GUID witch is aunique id allowing the cross scene references.

# I/O system

The I/O system is modelled after what Valve's source engien uses. The core idea is that game objects can send events to each other. An event comes from an Output located on a c# script and is delivered to an Input on an other script.

To create 