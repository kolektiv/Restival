﻿using Restival.Api.Common.Resources;

namespace Restival.Api.OpenRasta.Handlers {
    public class HelloHandler {
        public object Get(string name) {
            return (new Greeting(name));
        }
    }
}