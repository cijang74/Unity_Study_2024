interface IWeapon
{
    // 해당 인터페이스를 상속받을 클래스에서 사용할 함수 이름 작성, 무조건 퍼블릭
    // 구현은 상속받을 클래스A에서, 클래스B에서 해당 인터페이스에 접근하여 함수 실행하면
    // 클래스 A에서 구현한 함수 실행됨.
    public void Attack();
    public WeaponInfo GetWeaponInfo();
}