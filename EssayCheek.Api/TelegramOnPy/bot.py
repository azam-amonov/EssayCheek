import logging
import sys
import asyncio

from aiogram import Bot, Dispatcher, types, exceptions
import requests
from aiogram.enums import ParseMode
from aiogram.filters import CommandStart

URL = "https://06da-213-230-102-68.ngrok-free.app/apiHome"

bot = Bot(token="6750350401:AAFgcHkE3LCYjomJFndPuBpX0kxy6G9n8c0")
token = "6750350401:AAFgcHkE3LCYjomJFndPuBpX0kxy6G9n8c0"
URL = "https://localhost/5116/apiHome"
dp = Dispatcher(bot=bot)


@dp.message(CommandStart())
async def command_start(message: types.Message) -> None:
    await message.answer("Hello there!", parse_mode="markdown")


@dp.message()
async def echo(message: types.Message) -> None:
    try:
        await message.reply(f"Your essay {message.text}")
        res = requests.post(url=URL, data=message.text, headers= {'Content-Type': 'text/plain'})
        print(f"Response: {res.text}")
    except TypeError:
        await message.answer("Nice try")

async def main() -> None:
    bot = Bot(token=token, parse_mode= ParseMode.MARKDOWN)
    await dp.start_polling(bot)

if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO, stream=sys.stdout)
    asyncio.run(main())