// See https://aka.ms/new-console-template for more information

using ClipboardManager.App.Platform;
using ClipboardManager.App.ViewModels;
using ClipboardManager.Core.Repositories;
using ClipboardManager.Core.Repositories.Contracts;
using ClipboardManager.Core.Services;
using ClipboardManager.Core.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection()
    .AddSingleton<IClipboardMonitor, MacClipboardMonitor>()
    .AddSingleton<IHotkeyService, MacHotkeyService>()
    .AddSingleton<IHistoryRepository, JsonHistoryRepository>()
    .AddSingleton<HistoryManager>()
    .AddSingleton<HistoryViewModel>()
    .BuildServiceProvider();