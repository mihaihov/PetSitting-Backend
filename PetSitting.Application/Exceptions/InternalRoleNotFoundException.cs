namespace PetSitting.Application.Exceptions
{
    public class InternalRoleNotFoundException : Exception
    {
        public InternalRoleNotFoundException(string roleName) 
            : base($"Internal role named {roleName} does not exist.") {}
    }
}