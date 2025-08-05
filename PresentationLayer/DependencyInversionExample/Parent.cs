namespace PresentationLayer.DependencyInversionExample
{
    public class Parent      // High Level Module
    {
        //public Child Child { get; set; }    // Low Level Module

        public IChild Child { get; set; }

        //public Parent()
        //{
        //    Child = new Child();   ==> ❌❌ tightly coupled ( Composition )
        //}

        public Parent(IChild child)  // Dependency Injection through Constructor
        {
            Child = child;    // Aggregation
        }
    }

}


// High ( Parent ) ----depend----> Low ( Child )


// Tightly Coupled


// Abstraction Layer ( Abstract CLass , INterface )


// Parent  --->  Child

// Parent , Child   ---> Abstraction Layer