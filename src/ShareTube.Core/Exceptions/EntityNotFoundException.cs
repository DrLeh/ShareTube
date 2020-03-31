using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Exceptions
{
    public abstract class ShareTubeFieldExceptionBase : Exception
    {
        public ShareTubeFieldExceptionBase() : base() { }
        public ShareTubeFieldExceptionBase(string message) : base(message) { }
    }

    public class EntityNotFoundException : ShareTubeFieldExceptionBase
    {
        public EntityNotFoundException(long id, string typeName) : base($"{typeName} with Id={id} not found") { }
        public EntityNotFoundException(string key, string keyName, string typeName) : base($"{typeName} with {keyName}={key} not found") { }
    }
}
