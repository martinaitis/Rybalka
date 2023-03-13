using Microsoft.AspNetCore.Mvc;
using Moq;
using Rybalka.Core.Dto.FishingNote;
using Rybalka.Core.Interfaces.Services;
using Rybalka.Test.Mocks.Database;
using Rybalka.Test.Utils;
using RybalkaWebAPI.Controllers;

namespace Rybalka.Test.TestControllers
{
    public sealed class FishingNoteControllerTest
    {
        [Fact]
        public async void GetNotesV2_WhenMultipleFiltersSet_BadRequestReturn()
        {
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None, 1, 1);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenIdExist_NoteReturn()
        {
            var mapper = Mappers.GetFishingNoteMapper();
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetFishingNoteById(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => mapper.Map<FishingNoteResponse>(MockFishingNoteTable.fishingNotes.First()));
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None, id: 1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenIdNotExist_NotFoundReturn()
        {
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetFishingNoteById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None, id: 999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenUserExist_NotesReturn()
        {
            var mapper = Mappers.GetFishingNoteMapper();
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetFishingNotesByUserId(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<FishingNoteResponse>()
                {
                    mapper.Map<FishingNoteResponse>(MockFishingNoteTable.fishingNotes.First())
                });
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None, userId: 1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenUserOrNotesNotExist_NoContentReturn()
        {
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetFishingNotesByUserId(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<FishingNoteResponse>());
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None, userId: 99);

            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenFiltersNotSetAndNotesExist_NotesReturn()
        {
            var mapper = Mappers.GetFishingNoteMapper();
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetAllFishingNotes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => mapper.Map<List<FishingNoteResponse>>(MockFishingNoteTable.fishingNotes));
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetNotesV2_WhenFiltersNotSetAndNotesNotExist_NoContentReturn()
        {
            var fishingNoteServiceMock = new Mock<IFishingNoteService>();
            fishingNoteServiceMock.Setup(m => m.GetAllFishingNotes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<FishingNoteResponse>());
            var controller = new FishingNoteController(fishingNoteServiceMock.Object);

            var result = await controller.GetNotes(CancellationToken.None);

            Assert.IsType<NoContentResult>(result.Result);
        }
    }
}
