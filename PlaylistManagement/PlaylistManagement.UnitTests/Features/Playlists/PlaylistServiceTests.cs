using Moq;
using PlaylistManagement.Api.Exceptions;
using PlaylistManagement.Api.Features.Playlists;
using PlaylistManagement.Api.Features.Playlists.DTOs;
using PlaylistManagement.Api.Models.Entities;
using PlaylistManagement.Api.Repositories.Interfaces;

namespace PlaylistManagement.UnitTests.Features.Playlists;

public class PlaylistServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreatePlaylistForUser()
    {
        //Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object, songRepositoryMock.Object);

        var request = new CreatePlaylistRequest
        {
            Name = "My Favorites"
        };

        var userId = Guid.NewGuid();

        //Act
        var result = await service.CreateAsync(request,userId);

        //Assert
        Assert.Equal("My Favorites", result.Name);

        playlistRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<Playlist>(p =>
                    p.Name == "My Favorites" &&
                    p.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    //to verify that white space is trimmed
    [Fact]
    public async Task CreateAsync_ShouldTrimPlaylistName()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var request = new CreatePlaylistRequest
        {
            Name = "  My Favorites  "
        };

        var userId = Guid.NewGuid();

        // Act
        var result = await service.CreateAsync(request, userId);

        // Assert
        Assert.Equal("My Favorites", result.Name);

        playlistRepositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<Playlist>(p =>
                    p.Name == "My Favorites" &&
                    p.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task AddSongAsync_ShouldAddSong_WhenPlaylistAndSongAreValid()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var userId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = playlistId,
            UserId = userId,
            Name = "Favorites"
        };

        var song = new Song
        {
            Id = songId,
            Title = "Blinding Lights",
            Artist = "The Weeknd"
        };

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(playlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        songRepositoryMock
            .Setup(r => r.GetByIdAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);

        playlistRepositoryMock
            .Setup(r => r.ContainsSongAsync(
                playlistId,
                songId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        await service.AddSongAsync(
            playlistId,
            new AddSongToPlaylistRequest
            {
                SongId = songId
            },
            userId);

        //Assert
        playlistRepositoryMock.Verify(
            r => r.AddSongAsync(
                It.Is<PlaylistSong>(ps =>
                    ps.PlaylistId == playlistId &&
                    ps.SongId == songId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task AddSongAsync_ShouldThrowNotFound_WhenPlaylistDoesNotExist()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(
                playlistId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Playlist?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.AddSongAsync(
                playlistId,
                new AddSongToPlaylistRequest { SongId = songId },
                userId));
    }
    [Fact]
    public async Task AddSongAsync_ShouldThrowForbidden_WhenPlaylistBelongsToAnotherUser()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = playlistId,
            UserId = ownerId,
            Name = "Private Playlist"
        };

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(
                playlistId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        //Act and Assert
        await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.AddSongAsync(
                playlistId,
                new AddSongToPlaylistRequest { SongId = songId },
                otherUserId));
    }
    [Fact]
    public async Task UpdateAsync_ShouldUpdatePlaylistName_WhenUserOwnsPlaylist()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var userId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = playlistId,
            UserId = userId,
            Name = "Old Name"
        };

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(
                playlistId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        // Act
        await service.UpdateAsync(
            playlistId,
            new UpdatePlaylistRequest
            {
                Name = "New Name"
            },
            userId);

        // Assert
        Assert.Equal("New Name", playlist.Name);

        playlistRepositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_ShouldThrowForbidden_WhenUserDoesNotOwnPlaylist()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var ownerId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(
                playlistId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Playlist
            {
                Id = playlistId,
                UserId = ownerId,
                Name = "Favorites"
            });

        // Act and Assert
        await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.UpdateAsync(
                playlistId,
                new UpdatePlaylistRequest { Name = "Hacked" },
                otherUserId));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePlaylist_WhenUserOwnsPlaylist()
    {
        // Arrange
        var playlistRepositoryMock = new Mock<IPlaylistRepository>();
        var songRepositoryMock = new Mock<ISongRepository>();

        var service = new PlaylistService(playlistRepositoryMock.Object,songRepositoryMock.Object);

        var userId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();

        var playlist = new Playlist
        {
            Id = playlistId,
            UserId = userId,
            Name = "Favorites"
        };

        playlistRepositoryMock
            .Setup(r => r.GetByIdAsync(
                playlistId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        // Act
        await service.DeleteAsync(
            playlistId,
            userId);

        // Assert
        playlistRepositoryMock.Verify(
            r => r.Remove(playlist),
            Times.Once);

        playlistRepositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
