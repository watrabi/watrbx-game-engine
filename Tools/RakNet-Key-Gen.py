import hashlib
import codecs

def generate_sha1_rot13_key(version, platform, product, salt):
    combined = version + platform + product + salt
    sha1_hash = hashlib.sha1(combined.encode()).hexdigest()
    rot13_hash = codecs.encode(sha1_hash, 'rot_13')

    return rot13_hash

def main():
    print("=== RakNet Security Key Generator ===")
    version = input("Enter version (e.g. 0.235.0): ").strip()
    platform = input("Enter platform (e.g. pc): ").strip()
    product = input("Enter product (e.g. player): ").strip()
    salt = input("Enter salt: ").strip()

    rot13_hash = generate_sha1_rot13_key(version, platform, product, salt)

    print("\nROT13 of SHA-1 Hash:")
    print(rot13_hash)

    print(f'\nC++ usage:\nRBX::Network::securityKey = RBX::rot13("{rot13_hash}");')

if __name__ == "__main__":
    main()
