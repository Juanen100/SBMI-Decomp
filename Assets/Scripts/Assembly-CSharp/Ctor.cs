public interface Ctor<Base>
{
	Base Create();

	Base Create(Identity id);
}
