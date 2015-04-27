﻿using Restival.Api.Common.Resources;
using ServiceStack;

namespace Restival.Api.ServiceStack.Services {
    public class HelloService : Service {
        public Greeting Get(HelloRequestDto dto) {
            if (dto.Name == null) return (new Greeting());
            return (new Greeting(dto.Name));
        }
    }
}