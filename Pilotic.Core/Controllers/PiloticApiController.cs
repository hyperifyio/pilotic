using Microsoft.AspNetCore.Mvc;

using Pilotic.Core.Interfaces;

namespace Pilotic.Core.Controllers;

public abstract class PiloticApiController : ControllerBase, IInjectableSingletonModule
{
}