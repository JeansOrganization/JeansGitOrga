public record Car(string carName, string? year, int price)
{
    // : this() 表示调用此构造函数会去再去调用同级符合参数的构造函数
    // : base() 表示调用此构造函数会去再去调用父类符合参数的构造函数
    public Car(string carName, int age) : this(carName, null, age) { }
}